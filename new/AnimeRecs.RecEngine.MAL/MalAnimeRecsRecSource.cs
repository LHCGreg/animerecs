using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.MalApi;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalAnimeRecsRecSource
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, AnimeRecsRecommendation>
    {
        private AnimeRecsRecSource<MalTrainingData, MalUserListEntries, MalUserListEntries> m_recommender;
        private MalPercentageRatingClassifier m_recommendationClassifier;
        private MalPercentageRatingClassifier m_inputClassifier;
        private int m_minEpisodesToClassifyIncomplete;
        private Dictionary<int, float> m_itemAverages = new Dictionary<int, float>();
        private MalTrainingData m_trainingData = new MalTrainingData();

        public MalAnimeRecsRecSource(int numRecommendersToUse, double fractionConsideredRecommended, double targetFraction,
            int minEpisodesToClassifyIncomplete)
        {
            m_recommender = new AnimeRecsRecSource<MalTrainingData, MalUserListEntries, MalUserListEntries>(numRecommendersToUse);
            m_recommendationClassifier = new MalPercentageRatingClassifier(fractionConsideredRecommended, minEpisodesToClassifyIncomplete);
            m_inputClassifier = new MalPercentageRatingClassifier(targetFraction, minEpisodesToClassifyIncomplete);
            m_minEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
        }

        public void Train(MalTrainingData trainingData)
        {
            trainingData = FilterOutSpecials(trainingData);

            m_trainingData = trainingData;

            // Used for ordering recommendations from the same recommender with the same score
            SetAverages(trainingData);

            AnimeRecsTrainingData<MalTrainingData, MalUserListEntries> trainingDataWithParameters =
                new AnimeRecsTrainingData<MalTrainingData, MalUserListEntries>(trainingData, m_recommendationClassifier);
            m_recommender.Train(trainingDataWithParameters);
        }

        private MalTrainingData FilterOutSpecials(MalTrainingData trainingData)
        {
            Dictionary<int, MalUserListEntries> filteredUsers = new Dictionary<int, MalUserListEntries>();
            foreach (int userId in trainingData.Users.Keys)
            {
                Dictionary<int, MalListEntry> filteredEntries = new Dictionary<int, MalListEntry>();
                foreach (int animeId in trainingData.Users[userId].Entries.Keys)
                {
                    if (trainingData.Animes[animeId].Type != MalAnimeType.Special)
                    {
                        filteredEntries[animeId] = trainingData.Users[userId].Entries[animeId];
                    }
                }

                MalUserListEntries filteredUser = new MalUserListEntries(filteredEntries, trainingData.Users[userId].AnimesEligibleForRecommendation, trainingData.Users[userId].MalUsername);
                filteredUsers[userId] = filteredUser;
            }

            MalTrainingData filteredTrainingData = new MalTrainingData(filteredUsers, trainingData.Animes);
            return filteredTrainingData;
        }

        private void SetAverages(MalTrainingData trainingData)
        {
            m_itemAverages.Clear();

            Dictionary<int, float> scoreSumByItem = new Dictionary<int, float>();
            Dictionary<int, int> numScoresByItem = new Dictionary<int, int>();

            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(m_minEpisodesToClassifyIncomplete, includeDropped: true);

            foreach (int userId in basicTrainingData.Users.Keys)
            {
                foreach (KeyValuePair<int, float> itemIdRatingPair in basicTrainingData.Users[userId].Ratings)
                {
                    int itemId = itemIdRatingPair.Key;
                    float rating = itemIdRatingPair.Value;

                    if (!scoreSumByItem.ContainsKey(itemId))
                    {
                        scoreSumByItem[itemId] = 0;
                        numScoresByItem[itemId] = 0;
                    }

                    scoreSumByItem[itemId] += rating;
                    numScoresByItem[itemId]++;
                }
            }

            foreach (int itemId in scoreSumByItem.Keys)
            {
                float averageScore = scoreSumByItem[itemId] / numScoresByItem[itemId];
                m_itemAverages[itemId] = averageScore;
            }
        }

        public IEnumerable<AnimeRecsRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            AnimeRecsInput<MalUserListEntries> inputWithParameters = new AnimeRecsInput<MalUserListEntries>(
                originalInput: inputForUser,
                classifiedInput: m_inputClassifier.Classify(inputForUser),
                orderingGivenRecommenderAndItemIds: CompareRecs
            );
            return m_recommender.GetRecommendations(inputWithParameters, numRecommendationsToTryToGet);
        }

        // Order recommendations by one recommender by recommender's score, then by average score.
        private int CompareRecs(Tuple<int, int> recommenderAndItemId1, Tuple<int, int> recommenderAndItemId2)
        {
            int userId = recommenderAndItemId1.Item1;
            int itemId1 = recommenderAndItemId1.Item2;
            int itemId2 = recommenderAndItemId2.Item2;

            decimal item1Score = m_trainingData.Users[userId].Entries[itemId1].Rating.Value;
            decimal item2Score = m_trainingData.Users[userId].Entries[itemId2].Rating.Value;

            if (item1Score > item2Score)
            {
                return 1;
            }
            else if (item1Score < item2Score)
            {
                return -1;
            }
            else
            {
                return m_itemAverages[itemId1].CompareTo(m_itemAverages[itemId2]);
            }
        }

        public override string ToString()
        {
            return string.Format("AnimeRecs NumRecommendersToUse = {0} FractionConsideredRecommended = {1} TargetFraction = {2}, MinEpisodesToClassifyIncomplete = {3}",
                m_recommender.NumRecommenders, m_recommendationClassifier.GoodFraction, m_inputClassifier.GoodFraction,
                m_minEpisodesToClassifyIncomplete);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.