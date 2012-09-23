using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.ItemRecommendation;
using MyMediaLite.DataType;
using MyMediaLite.Data;

namespace AnimeRecs.RecEngine
{
    public class MyMediaLiteItemRecommendationRecSource<TRecommender>
        : ITrainableRecSource<IBasicTrainingData<IPositiveFeedbackForUser>, IPositiveFeedbackForUser,
        IEnumerable<RatingPredictionRecommendation>, RatingPredictionRecommendation>

        where TRecommender : ItemRecommender, IFoldInItemRecommender
    {
        private TRecommender m_recommender;

        private Dictionary<int, int> m_realUserIdToMediaLiteUserId = new Dictionary<int, int>();
        private Dictionary<int, int> m_mediaLiteUserIdToRealUserId = new Dictionary<int, int>();
        private int m_nextMediaLiteUserId = 0;

        private Dictionary<int, int> m_realItemIdToMediaLiteItemId = new Dictionary<int, int>();
        private Dictionary<int, int> m_mediaLiteItemIdToRealItemId = new Dictionary<int, int>();
        private int m_nextMediaLiteItemId = 0;

        public MyMediaLiteItemRecommendationRecSource(TRecommender recommender)
        {
            m_recommender = recommender;
        }
        
        public void Train(IBasicTrainingData<IPositiveFeedbackForUser> trainingData)
        {
            m_realUserIdToMediaLiteUserId = new Dictionary<int, int>();
            m_mediaLiteUserIdToRealUserId = new Dictionary<int, int>();
            m_nextMediaLiteUserId = 0;

            m_realItemIdToMediaLiteItemId = new Dictionary<int, int>();
            m_mediaLiteItemIdToRealItemId = new Dictionary<int, int>();
            m_nextMediaLiteItemId = 0;

            PosOnlyFeedback<SparseBooleanMatrix> mediaLiteFeedback = new PosOnlyFeedback<SparseBooleanMatrix>();
            foreach(KeyValuePair<int, IPositiveFeedbackForUser> userFeedbackPair in trainingData.Users)
            {
                int userId = userFeedbackPair.Key;
                IPositiveFeedbackForUser feedback = userFeedbackPair.Value;

                m_realUserIdToMediaLiteUserId[userId] = m_nextMediaLiteUserId;
                m_mediaLiteUserIdToRealUserId[m_nextMediaLiteUserId] = userId;
                m_nextMediaLiteUserId++;

                foreach(int itemId in feedback.Items)
                {
                    if (!m_realItemIdToMediaLiteItemId.ContainsKey(itemId))
                    {
                        m_realItemIdToMediaLiteItemId[itemId] = m_nextMediaLiteItemId;
                        m_mediaLiteItemIdToRealItemId[m_nextMediaLiteItemId] = itemId;
                        m_nextMediaLiteItemId++;
                    }

                    mediaLiteFeedback.Add(m_realUserIdToMediaLiteUserId[userId], m_realItemIdToMediaLiteItemId[itemId]);
                } 
            }
            
            m_recommender.Feedback = mediaLiteFeedback;
            m_recommender.Train();
        }

        public IEnumerable<RatingPredictionRecommendation> GetRecommendations(IPositiveFeedbackForUser inputForUser, int numRecommendationsToTryToGet)
        {
            IList<int> userMediaLiteFeedback = new List<int>();
            foreach(int realItemId in inputForUser.Items)
            {
                // Do not pass in items that MyMediaLite does not know about, it will crash
                if (m_realItemIdToMediaLiteItemId.ContainsKey(realItemId))
                {
                    int mediaLiteItemId = m_realItemIdToMediaLiteItemId[realItemId];
                    userMediaLiteFeedback.Add(mediaLiteItemId);
                }
            }

            List<int> mediaLiteCandidateItemIds = m_realItemIdToMediaLiteItemId.Keys
                .Where(realItemId => inputForUser.ItemIsOkToRecommend(realItemId))
                .Select(realItemId => m_realItemIdToMediaLiteItemId[realItemId])
                .ToList();
            IList<Tuple<int, float>> mediaLiteScores = m_recommender.ScoreItems(userMediaLiteFeedback, mediaLiteCandidateItemIds);

            List<RatingPredictionRecommendation> recs = new List<RatingPredictionRecommendation>();
            foreach (Tuple<int, float> score in mediaLiteScores.OrderByDescending(p => p.Item2))
            {
                int mediaLiteItemId = score.Item1;
                float predictedScore = score.Item2;

                int realItemId = m_mediaLiteItemIdToRealItemId[mediaLiteItemId];
                recs.Add(new RatingPredictionRecommendation(realItemId, predictedScore));
                if (recs.Count >= numRecommendationsToTryToGet)
                {
                    break;
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