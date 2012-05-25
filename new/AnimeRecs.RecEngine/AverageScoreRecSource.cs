using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class AverageScoreRecSource<TTrainingData, TTrainingDataUserRatings>
        : IRecommendationSource<IInputForUser, AverageScoreRecommendation>, ITrainable<TTrainingData>,
        ITrainableRecSource<TTrainingData, IInputForUser, AverageScoreRecommendation>

        where TTrainingData : IBasicTrainingData<TTrainingDataUserRatings>
        where TTrainingDataUserRatings : IBasicInputForUser
    {
        private List<Tuple<int, int, float>> m_itemIdNumRatingsAndAverage = new List<Tuple<int, int, float>>();

        public AverageScoreRecSource()
        {
            ;
        }

        public void Train(TTrainingData trainingData)
        {
            m_itemIdNumRatingsAndAverage.Clear();
            
            Dictionary<int, float> scoreSumByItem = new Dictionary<int, float>();
            Dictionary<int, int> numScoresByItem = new Dictionary<int, int>();

            foreach (int userId in trainingData.Users.Keys)
            {
                foreach (KeyValuePair<int, float> itemIdRatingPair in trainingData.Users[userId].Ratings)
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
                m_itemIdNumRatingsAndAverage.Add(new Tuple<int, int, float>(itemId, numScoresByItem[itemId], averageScore));
            }

            m_itemIdNumRatingsAndAverage.Sort((x, y) => -(x.Item3.CompareTo(y.Item3)));
        }

        public IEnumerable<AverageScoreRecommendation> GetRecommendations(IInputForUser userRatings, int numRecommendationsToTryToGet)
        {
            List<AverageScoreRecommendation> recs = new List<AverageScoreRecommendation>();
            for (int i = 0; recs.Count < numRecommendationsToTryToGet && i < m_itemIdNumRatingsAndAverage.Count; i++)
            {
                // Only recommend items the user has not already rated
                if (userRatings.ItemIsOkToRecommend(m_itemIdNumRatingsAndAverage[i].Item1))
                {
                    recs.Add(new AverageScoreRecommendation(m_itemIdNumRatingsAndAverage[i].Item1,
                        m_itemIdNumRatingsAndAverage[i].Item2, m_itemIdNumRatingsAndAverage[i].Item3));
                }
            }

            return recs;
        }
    }

    public class AverageScoreRecommendation : IRecommendation
    {
        public int ItemId { get; private set; }
        public int NumRatings { get; private set; }
        public float AverageScore { get; private set; }

        public AverageScoreRecommendation(int itemId, int numRatings, float averageScore)
        {
            ItemId = itemId;
            NumRatings = numRatings;
            AverageScore = averageScore;
        }

        public override string ToString()
        {
            return AverageScore.ToString();
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.