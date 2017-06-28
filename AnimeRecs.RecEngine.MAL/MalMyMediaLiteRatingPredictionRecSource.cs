#if MYMEDIALITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.RatingPrediction;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMyMediaLiteRatingPredictionRecSource<TRecommender>
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<RatingPredictionRecommendation>, RatingPredictionRecommendation>
        where TRecommender : RatingPredictor, IFoldInRatingPredictor
    {
        private MyMediaLiteRatingPredictionRecSource<TRecommender> m_recommender;
        private Dictionary<int, int> m_userCountByAnime;
        private int m_minEpisodesToCountIncomplete;
        private bool m_useDropped;
        private int m_minUsersToCountAnime;

        public MalMyMediaLiteRatingPredictionRecSource(TRecommender recommender, int minEpisodesToCountIncomplete, bool useDropped,
            int minUsersToCountAnime)
        {
            m_recommender = new MyMediaLiteRatingPredictionRecSource<TRecommender>(recommender);
            m_userCountByAnime = null;
            m_minEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            m_useDropped = useDropped;
            m_minUsersToCountAnime = minUsersToCountAnime;
        }

        public void Train(MalTrainingData trainingData)
        {
            m_userCountByAnime = new Dictionary<int,int>();
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(m_minEpisodesToCountIncomplete, m_useDropped);

            foreach (int userId in basicTrainingData.Users.Keys)
            {
                foreach (int itemId in basicTrainingData.Users[userId].Ratings.Keys)
                {
                    if (!m_userCountByAnime.ContainsKey(itemId))
                    {
                        m_userCountByAnime[itemId] = 0;
                    }
                    m_userCountByAnime[itemId]++;
                }
            }

            // Do not filter out anime with few users at this point. Let the MyMediaLite recommender possibly make use of that data.

            m_recommender.Train(basicTrainingData);
        }

        public IEnumerable<RatingPredictionRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            IBasicInputForUser basicInput = inputForUser.AsBasicInput(m_minEpisodesToCountIncomplete, m_useDropped,
                additionalOkToRecommendPredicate: (animeId) => m_userCountByAnime.ContainsKey(animeId) && m_userCountByAnime[animeId] >= m_minUsersToCountAnime
            );

            return m_recommender.GetRecommendations(basicInput, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("{0} MinEpisodesToCountIncomplete = {1}, UseDropped = {2}, MinUsersToCountAnime = {3}",
                m_recommender, m_minEpisodesToCountIncomplete, m_useDropped, m_minUsersToCountAnime);
        }
    }
}

#endif

// Copyright (C) 2017 Greg Najda
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