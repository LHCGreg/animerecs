using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using MyMediaLite.RatingPrediction;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Recommendation source that uses a MyMediaLite rating prediction algorithm.
    /// </summary>
    /// <typeparam name="TRecommender"></typeparam>
    public class MyMediaLiteRatingPredictionRecSource<TRecommender>
        : ITrainableRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser,
        IEnumerable<RatingPredictionRecommendation>, RatingPredictionRecommendation>

        where TRecommender : RatingPredictor, IFoldInRatingPredictor
    {
        private TRecommender m_recommender;

        private Dictionary<int, int> m_realUserIdToMediaLiteUserId = new Dictionary<int, int>();
        private Dictionary<int, int> m_mediaLiteUserIdToRealUserId = new Dictionary<int, int>();
        private int m_nextMediaLiteUserId = 0;

        private Dictionary<int, int> m_realItemIdToMediaLiteItemId = new Dictionary<int, int>();
        private Dictionary<int, int> m_mediaLiteItemIdToRealItemId = new Dictionary<int, int>();
        private int m_nextMediaLiteItemId = 0;

        public MyMediaLiteRatingPredictionRecSource(TRecommender recommender)
        {
            m_recommender = recommender;
        }

        public void Train(IBasicTrainingData<IBasicInputForUser> trainingData)
        {
            m_realUserIdToMediaLiteUserId = new Dictionary<int, int>();
            m_mediaLiteUserIdToRealUserId = new Dictionary<int, int>();
            m_nextMediaLiteUserId = 0;

            m_realItemIdToMediaLiteItemId = new Dictionary<int, int>();
            m_mediaLiteItemIdToRealItemId = new Dictionary<int, int>();
            m_nextMediaLiteItemId = 0;

            MyMediaLite.Data.Ratings mediaLiteRatings = new MyMediaLite.Data.Ratings();
            foreach (KeyValuePair<int, IBasicInputForUser> userRatingsPair in trainingData.Users)
            {
                int userId = userRatingsPair.Key;
                IBasicInputForUser ratings = userRatingsPair.Value;

                m_realUserIdToMediaLiteUserId[userId] = m_nextMediaLiteUserId;
                m_mediaLiteUserIdToRealUserId[m_nextMediaLiteUserId] = userId;
                m_nextMediaLiteUserId++;

                foreach (KeyValuePair<int, float> rating in ratings.Ratings)
                {
                    int itemId = rating.Key;
                    float score = rating.Value;

                    if (!m_realItemIdToMediaLiteItemId.ContainsKey(itemId))
                    {
                        m_realItemIdToMediaLiteItemId[itemId] = m_nextMediaLiteItemId;
                        m_mediaLiteItemIdToRealItemId[m_nextMediaLiteItemId] = itemId;
                        m_nextMediaLiteItemId++;
                    }

                    mediaLiteRatings.Add(m_realUserIdToMediaLiteUserId[userId], m_realItemIdToMediaLiteItemId[itemId], score);
                }
            }

            m_recommender.Ratings = mediaLiteRatings;
            m_recommender.Train();
        }

        public IEnumerable<RatingPredictionRecommendation> GetRecommendations(IBasicInputForUser userRatings, int numRecommendationsToTryToGet)
        {
            IList<Tuple<int, float>> userMediaLiteRatings = new List<Tuple<int, float>>();
            foreach (KeyValuePair<int, float> realRating in userRatings.Ratings)
            {
                int realItemId = realRating.Key;
                float score = realRating.Value;

                // Do not pass in items that MyMediaLite does not know about, it will crash
                if (m_realItemIdToMediaLiteItemId.ContainsKey(realItemId))
                {
                    int mediaLiteItemId = m_realItemIdToMediaLiteItemId[realItemId];
                    userMediaLiteRatings.Add(new Tuple<int, float>(mediaLiteItemId, score));
                }
            }

            IList<Tuple<int, float>> mediaLitePredictions = m_recommender.ScoreItems(userMediaLiteRatings);

            List<RatingPredictionRecommendation> recs = new List<RatingPredictionRecommendation>();
            foreach (Tuple<int, float> prediction in mediaLitePredictions.OrderByDescending(p => p.Item2))
            {
                int mediaLiteItemId = prediction.Item1;
                float predictedScore = prediction.Item2;

                int realItemId = m_mediaLiteItemIdToRealItemId[mediaLiteItemId];
                if (userRatings.ItemIsOkToRecommend(realItemId))
                {
                    recs.Add(new RatingPredictionRecommendation(realItemId, predictedScore));
                    if (recs.Count >= numRecommendationsToTryToGet)
                    {
                        break;
                    }
                }
            }

            return recs;
        }

        public override string ToString()
        {
            return m_recommender.ToString();
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MyMediaLite.
//
// AnimeRecs.RecEngine.MyMediaLite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MyMediaLite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MyMediaLite.  If not, see <http://www.gnu.org/licenses/>.