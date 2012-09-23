using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.ItemRecommendation;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMyMediaLiteItemRecommenderRecSource<TRecommender>
        : ITrainableRecSource<MalTrainingData, MalPositiveFeedbackInput, MalPositiveFeedbackRecResults, RatingPredictionRecommendation>

        where TRecommender : ItemRecommender, IFoldInItemRecommender
    {
        private MyMediaLiteItemRecommendationRecSource<TRecommender> m_recommender;

        private Dictionary<int, int> m_userCountByAnime;
        private int m_minEpisodesToClassifyIncomplete;
        private int m_minUsersToCountAnime;
        private MalPercentageRatingClassifier m_positiveClassifier;

        public MalMyMediaLiteItemRecommenderRecSource(TRecommender recommender, double fractionConsideredRecommended,
            int minEpisodesToClassifyIncomplete, int minUsersToCountAnime)
        {
            m_recommender = new MyMediaLiteItemRecommendationRecSource<TRecommender>(recommender);
            m_userCountByAnime = null;
            m_minEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
            m_minUsersToCountAnime = minUsersToCountAnime;
            m_positiveClassifier = new MalPercentageRatingClassifier(fractionConsideredRecommended, minEpisodesToClassifyIncomplete);
        }

        public void Train(MalTrainingData trainingData)
        {
            m_userCountByAnime = new Dictionary<int, int>();

            IBasicTrainingData<IPositiveFeedbackForUser> basicFeedback = trainingData.AsPositiveFeedback(m_positiveClassifier);

            foreach (int userId in basicFeedback.Users.Keys)
            {
                foreach (int itemId in basicFeedback.Users[userId].Items)
                {
                    if (!m_userCountByAnime.ContainsKey(itemId))
                    {
                        m_userCountByAnime[itemId] = 0;
                    }
                    m_userCountByAnime[itemId]++;
                }
            }

            // Do not filter out anime with few users at this point. Let the MyMediaLite recommender possibly make use of that data.

            m_recommender.Train(basicFeedback);
        }

        public MalPositiveFeedbackRecResults GetRecommendations(MalPositiveFeedbackInput inputForUser, int numRecommendationsToTryToGet)
        {
            IUserInputClassifier<MalUserListEntries> classifier;
            decimal targetScoreUsed;
            if (inputForUser.TargetFraction != null)
            {
                classifier = new MalPercentageRatingClassifier(inputForUser.TargetFraction.Value, m_minEpisodesToClassifyIncomplete);
            }
            else
            {
                classifier = new MalMinimumScoreRatingClassifier(inputForUser.TargetScore.Value, m_minEpisodesToClassifyIncomplete);
            }

            IPositiveFeedbackForUser basicFeedback = inputForUser.AnimeList.AsPositiveFeedback(classifier,
            additionalOkToRecommendPredicate: (animeId) => m_userCountByAnime.ContainsKey(animeId) && m_userCountByAnime[animeId] >= m_minUsersToCountAnime);

            if (inputForUser.TargetFraction != null)
            {
                targetScoreUsed = basicFeedback.Items.Min(itemId => inputForUser.AnimeList.Entries[itemId].Rating ?? 10);
            }
            else
            {
                targetScoreUsed = inputForUser.TargetScore.Value;
            }

            IEnumerable<RatingPredictionRecommendation> recs = m_recommender.GetRecommendations(basicFeedback, numRecommendationsToTryToGet);
            return new MalPositiveFeedbackRecResults(recs, targetScoreUsed);
        }

        public override string ToString()
        {
            return string.Format("{0} MinEpisodesToClassifyIncomplete = {1}, FractionConsideredRecommended = {2}, MinUsersToCountAnime = {3}",
                m_recommender, m_minEpisodesToClassifyIncomplete, m_positiveClassifier.GoodFraction, m_minUsersToCountAnime);
        }
    }

    public class MalMyMediaLiteItemRecommenderRecSourceWithConstantPercentTarget<TRecommender>
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, MalPositiveFeedbackRecResults, RatingPredictionRecommendation>

        where TRecommender : ItemRecommender, IFoldInItemRecommender
    {
        private MalMyMediaLiteItemRecommenderRecSource<TRecommender> m_underlyingRecSource;
        private double m_targetFraction;
        
        public MalMyMediaLiteItemRecommenderRecSourceWithConstantPercentTarget(TRecommender recommender, double fractionConsideredRecommended,
            int minEpisodesToClassifyIncomplete, int minUsersToCountAnime, double targetFraction)
        {
            m_underlyingRecSource = new MalMyMediaLiteItemRecommenderRecSource<TRecommender>(
                recommender: recommender,
                fractionConsideredRecommended: fractionConsideredRecommended,
                minEpisodesToClassifyIncomplete: minEpisodesToClassifyIncomplete,
                minUsersToCountAnime: minUsersToCountAnime
            );
            m_targetFraction = targetFraction;
        }

        public void Train(MalTrainingData trainingData)
        {
            m_underlyingRecSource.Train(trainingData);
        }

        public MalPositiveFeedbackRecResults GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            MalPositiveFeedbackInput input = new MalPositiveFeedbackInput(inputForUser, targetFraction: m_targetFraction);
            return m_underlyingRecSource.GetRecommendations(input, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return m_underlyingRecSource.ToString() + string.Format(", TargetFraction={0}", m_targetFraction);
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