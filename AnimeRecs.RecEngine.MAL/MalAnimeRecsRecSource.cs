using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;
using AnimeRecs.RecEngine.Utils;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalAnimeRecsInput : IInputForUser
    {
        public MalUserListEntries AnimeList { get; private set; }
        public double? TargetFraction { get; private set; }
        public decimal? TargetScore { get; private set; }

        public MalAnimeRecsInput(MalUserListEntries animeList, double targetFraction)
        {
            AnimeList = animeList;
            TargetFraction = targetFraction;
        }

        public MalAnimeRecsInput(MalUserListEntries animeList, decimal targetScore)
        {
            AnimeList = animeList;
            TargetScore = targetScore;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return AnimeList.ItemIsOkToRecommend(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return AnimeList.ContainsItem(itemId);
        }
    }
    
    public class MalAnimeRecsRecSource
        : ITrainableRecSource<MalTrainingData, MalAnimeRecsInput, AnimeRecsResults, AnimeRecsRecommendation>,
          ITrainableRecSource<MalTrainingData, MalAnimeRecsInput, MalAnimeRecsResults, AnimeRecsRecommendation>
    {
        private AnimeRecsRecSource<MalTrainingData, MalUserListEntries, MalUserListEntries> m_recommender;
        private MalPercentageRatingClassifier m_recommendationClassifier;
        private int m_minEpisodesToClassifyIncomplete;
        private Dictionary<int, float> m_itemAverages = new Dictionary<int, float>();
        private MalTrainingData m_trainingData = new MalTrainingData();

        public MalAnimeRecsRecSource(int numRecommendersToUse, double fractionConsideredRecommended, int minEpisodesToClassifyIncomplete)
        {
            m_recommender = new AnimeRecsRecSource<MalTrainingData, MalUserListEntries, MalUserListEntries>(numRecommendersToUse);
            m_recommendationClassifier = new MalPercentageRatingClassifier(fractionConsideredRecommended, minEpisodesToClassifyIncomplete);
            m_minEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
        }

        public void Train(MalTrainingData trainingData)
        {
            trainingData = FilterOutSpecialsAndTrim(trainingData);

            m_trainingData = trainingData;

            // Used for ordering recommendations from the same recommender with the same score
            SetAverages(trainingData);

            AnimeRecsTrainingData<MalTrainingData, MalUserListEntries> trainingDataWithParameters =
                new AnimeRecsTrainingData<MalTrainingData, MalUserListEntries>(trainingData, m_recommendationClassifier);
            m_recommender.Train(trainingDataWithParameters);
        }

        private MalTrainingData FilterOutSpecialsAndTrim(MalTrainingData trainingData)
        {
            Dictionary<int, MalUserListEntries> filteredUsers = new Dictionary<int, MalUserListEntries>();
            foreach (int userId in trainingData.Users.Keys.Take(m_recommender.NumRecommenders))
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

        AnimeRecsResults IRecommendationSource<MalAnimeRecsInput, AnimeRecsResults, AnimeRecsRecommendation>
            .GetRecommendations(MalAnimeRecsInput inputForUser, int numRecommendationsToTryToGet)
        {
            ClassifiedUserInput<MalUserListEntries> classifiedInput;
            if (inputForUser.TargetFraction != null)
            {
                MalPercentageRatingClassifier inputClassifier = new MalPercentageRatingClassifier(inputForUser.TargetFraction.Value, m_minEpisodesToClassifyIncomplete);
                classifiedInput = inputClassifier.Classify(inputForUser.AnimeList);
            }
            else
            {
                MalMinimumScoreRatingClassifier inputClassifier = new MalMinimumScoreRatingClassifier(inputForUser.TargetScore.Value, m_minEpisodesToClassifyIncomplete);
                classifiedInput = inputClassifier.Classify(inputForUser.AnimeList);
            }

            AnimeRecsInput<MalUserListEntries> inputWithParameters = new AnimeRecsInput<MalUserListEntries>(
                originalInput: inputForUser.AnimeList,
                classifiedInput: classifiedInput,
                orderingGivenRecommenderAndItemIds: CompareRecs
            );
            return m_recommender.GetRecommendations(inputWithParameters, numRecommendationsToTryToGet);
        }

        public MalAnimeRecsResults GetRecommendations(MalAnimeRecsInput inputForUser, int numRecommendationsToTryToGet)
        {
            ClassifiedUserInput<MalUserListEntries> classifiedInput;
            decimal targetScoreUsed;
            if (inputForUser.TargetFraction != null)
            {
                MalPercentageRatingClassifier inputClassifier = new MalPercentageRatingClassifier(inputForUser.TargetFraction.Value, m_minEpisodesToClassifyIncomplete);
                classifiedInput = inputClassifier.Classify(inputForUser.AnimeList);

                // target score used is the minimum score a liked anime.
                targetScoreUsed = classifiedInput.Liked.Entries.Values.Min(entry => entry.Rating) ?? 10m;
            }
            else
            {
                MalMinimumScoreRatingClassifier inputClassifier = new MalMinimumScoreRatingClassifier(inputForUser.TargetScore.Value, m_minEpisodesToClassifyIncomplete);
                classifiedInput = inputClassifier.Classify(inputForUser.AnimeList);
                targetScoreUsed = inputForUser.TargetScore.Value;
            }

            AnimeRecsInput<MalUserListEntries> inputWithParameters = new AnimeRecsInput<MalUserListEntries>(
                originalInput: inputForUser.AnimeList,
                classifiedInput: classifiedInput,
                orderingGivenRecommenderAndItemIds: CompareRecs
            );

            AnimeRecsResults baseResults = m_recommender.GetRecommendations(inputWithParameters, numRecommendationsToTryToGet);

            List<MalAnimeRecsRecommenderUser> malRecommenders = new List<MalAnimeRecsRecommenderUser>();
            foreach(AnimeRecsRecommenderUser baseRecommender in baseResults.Recommenders)
            {
                malRecommenders.Add(new MalAnimeRecsRecommenderUser(
                    userId: baseRecommender.UserId,
                    username: m_trainingData.Users[baseRecommender.UserId].MalUsername,
                    recsLiked: new HashSet<MalAnimeRecsRecommenderRecommendation>(baseRecommender.RecsLiked
                        .Select(malAnimeId => new MalAnimeRecsRecommenderRecommendation(malAnimeId,
                            m_trainingData.Users[baseRecommender.UserId].Entries[malAnimeId].Rating, m_itemAverages[malAnimeId])
                        )
                    ),
                    recsNotLiked: new HashSet<MalAnimeRecsRecommenderRecommendation>(baseRecommender.RecsNotLiked
                        .Select(malAnimeId => new MalAnimeRecsRecommenderRecommendation(malAnimeId,
                            m_trainingData.Users[baseRecommender.UserId].Entries[malAnimeId].Rating, m_itemAverages[malAnimeId])
                        )
                    ),
                    recsInconclusive: new HashSet<MalAnimeRecsRecommenderRecommendation>(baseRecommender.RecsInconclusive
                        .Select(malAnimeId => new MalAnimeRecsRecommenderRecommendation(malAnimeId,
                            m_trainingData.Users[baseRecommender.UserId].Entries[malAnimeId].Rating, m_itemAverages[malAnimeId])
                        )
                    ),
                    recsNotInCommon: new HashSet<MalAnimeRecsRecommenderRecommendation>(baseRecommender.RecsNotInCommon
                        .Select(malAnimeId => new MalAnimeRecsRecommenderRecommendation(malAnimeId,
                            m_trainingData.Users[baseRecommender.UserId].Entries[malAnimeId].Rating, m_itemAverages[malAnimeId])
                        )
                    ),
                    compatibility: baseRecommender.Compatibility,
                    compatibilityLowEndpoint: baseRecommender.CompatibilityLowEndpoint,
                    compatibilityHighEndpoint: baseRecommender.CompatibilityHighEndpoint
                ));
            }

            return new MalAnimeRecsResults(baseResults.Recommendations, malRecommenders, targetScoreUsed);
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
            return string.Format("AnimeRecs NumRecommendersToUse={0} FractionConsideredRecommended={1} MinEpisodesToClassifyIncomplete={2}",
                m_recommender.NumRecommenders, m_recommendationClassifier.GoodFraction, m_minEpisodesToClassifyIncomplete);
        }
    }

    public class MalAnimeRecsResults : IEnumerable<AnimeRecsRecommendation>
    {
        /// <summary>
        /// Contains no more than the amount of recommendations asked for.
        /// </summary>
        public IList<AnimeRecsRecommendation> Recommendations { get; private set; }

        /// <summary>
        /// Contains all recommenders sorted by compatibility low endpoint in descending order.
        /// </summary>
        public IList<MalAnimeRecsRecommenderUser> Recommenders { get; private set; }

        public decimal TargetScoreUsed { get; private set; }

        public MalAnimeRecsResults(IList<AnimeRecsRecommendation> recommendations, IList<MalAnimeRecsRecommenderUser> recommenders,
            decimal targetScoreUsed)
        {
            Recommendations = recommendations;
            Recommenders = recommenders;
            TargetScoreUsed = targetScoreUsed;
        }

        public IEnumerator<AnimeRecsRecommendation> GetEnumerator()
        {
            return Recommendations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Recommendations).GetEnumerator();
        }
    }

    public class MalAnimeRecsRecommenderUser
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public ICollection<MalAnimeRecsRecommenderRecommendation> RecsLiked { get; private set; }
        public ICollection<MalAnimeRecsRecommenderRecommendation> RecsNotLiked { get; private set; }
        public ICollection<MalAnimeRecsRecommenderRecommendation> RecsInconclusive { get; private set; }
        public ICollection<MalAnimeRecsRecommenderRecommendation> RecsNotInCommon { get; private set; }
        public int NumRecsWithJudgment { get { return RecsLiked.Count + RecsNotLiked.Count; } }
        public double? Compatibility { get; private set; }
        public double? CompatibilityLowEndpoint { get; private set; }
        public double? CompatibilityHighEndpoint { get; private set; }
        public ICollection<MalAnimeRecsRecommenderRecommendation> AllRecommendations { get; private set; }
        
        public MalAnimeRecsRecommenderUser(int userId, string username, ICollection<MalAnimeRecsRecommenderRecommendation> recsLiked,
            ICollection<MalAnimeRecsRecommenderRecommendation> recsNotLiked, ICollection<MalAnimeRecsRecommenderRecommendation> recsInconclusive,
            ICollection<MalAnimeRecsRecommenderRecommendation> recsNotInCommon, double? compatibility, double? compatibilityLowEndpoint,
            double? compatibilityHighEndpoint)
        {
            UserId = userId;
            Username = username;
            RecsLiked = recsLiked;
            RecsNotLiked = recsNotLiked;
            RecsInconclusive = recsInconclusive;
            RecsNotInCommon = recsNotInCommon;
            AllRecommendations = new HashSet<MalAnimeRecsRecommenderRecommendation>(recsLiked.Concat(recsNotLiked).Concat(recsInconclusive).Concat(recsNotInCommon));
            Compatibility = compatibility;
            CompatibilityLowEndpoint = compatibilityLowEndpoint;
            CompatibilityHighEndpoint = compatibilityHighEndpoint;
        }
    }

    public class MalAnimeRecsRecommenderRecommendation : IEquatable<MalAnimeRecsRecommenderRecommendation>
    {
        public int MalAnimeId { get; private set; }
        public decimal? RecommenderScore { get; private set; }
        public double AverageScore { get; private set; }

        public MalAnimeRecsRecommenderRecommendation(int malAnimeId, decimal? recommenderScore, double averageScore)
        {
            MalAnimeId = malAnimeId;
            RecommenderScore = recommenderScore;
            AverageScore = averageScore;
        }

        public override bool Equals(object obj)
        {
 	         return Equals(obj as MalAnimeRecsRecommenderRecommendation);
        }

        public bool Equals(MalAnimeRecsRecommenderRecommendation other)
        {
            if(other == null)
                return false;
            else
                return this.MalAnimeId == other.MalAnimeId;
        }

        public override int GetHashCode()
        {
 	         return MalAnimeId.GetHashCode();
        }
    }

    public class MalAnimeRecsRecSourceWithConstantPercentTarget
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, AnimeRecsResults, AnimeRecsRecommendation>,
          ITrainableRecSource<MalTrainingData, MalUserListEntries, MalAnimeRecsResults, AnimeRecsRecommendation>
    {
        private MalAnimeRecsRecSource m_underlyingRecSource;
        private double m_targetFraction;

        public MalAnimeRecsRecSourceWithConstantPercentTarget(int numRecommendersToUse, double fractionConsideredRecommended,
            double targetFraction, int minEpisodesToClassifyIncomplete)
        {
            m_underlyingRecSource = new MalAnimeRecsRecSource(numRecommendersToUse, fractionConsideredRecommended, minEpisodesToClassifyIncomplete);
            m_targetFraction = targetFraction;
        }

        public void Train(MalTrainingData trainingData)
        {
            m_underlyingRecSource.Train(trainingData);
        }

        AnimeRecsResults IRecommendationSource<MalUserListEntries, AnimeRecsResults, AnimeRecsRecommendation>
            .GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return ((ITrainableRecSource<MalTrainingData, MalAnimeRecsInput, AnimeRecsResults, AnimeRecsRecommendation>)m_underlyingRecSource)
                .GetRecommendations(new MalAnimeRecsInput(inputForUser, m_targetFraction), numRecommendationsToTryToGet);
        }

        public MalAnimeRecsResults GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_underlyingRecSource.GetRecommendations(new MalAnimeRecsInput(inputForUser, m_targetFraction), numRecommendationsToTryToGet);
        }
    }

    public class MalAnimeRecsRecSourceWithConstantAbsoluteTarget
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, AnimeRecsResults, AnimeRecsRecommendation>,
          ITrainableRecSource<MalTrainingData, MalUserListEntries, MalAnimeRecsResults, AnimeRecsRecommendation>
    {
        private MalAnimeRecsRecSource m_underlyingRecSource;
        private decimal m_targetScore;

        public MalAnimeRecsRecSourceWithConstantAbsoluteTarget(int numRecommendersToUse, double fractionConsideredRecommended,
            decimal targetScore, int minEpisodesToClassifyIncomplete)
        {
            m_underlyingRecSource = new MalAnimeRecsRecSource(numRecommendersToUse, fractionConsideredRecommended, minEpisodesToClassifyIncomplete);
            m_targetScore = targetScore;
        }

        public void Train(MalTrainingData trainingData)
        {
            m_underlyingRecSource.Train(trainingData);
        }

        AnimeRecsResults IRecommendationSource<MalUserListEntries, AnimeRecsResults, AnimeRecsRecommendation>
            .GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return ((ITrainableRecSource<MalTrainingData, MalAnimeRecsInput, AnimeRecsResults, AnimeRecsRecommendation>)m_underlyingRecSource)
                .GetRecommendations(new MalAnimeRecsInput(inputForUser, m_targetScore), numRecommendationsToTryToGet);
        }

        public MalAnimeRecsResults GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_underlyingRecSource.GetRecommendations(new MalAnimeRecsInput(inputForUser, m_targetScore), numRecommendationsToTryToGet);
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