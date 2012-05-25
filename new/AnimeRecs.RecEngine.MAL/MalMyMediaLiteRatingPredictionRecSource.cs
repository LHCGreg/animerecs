using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.RatingPrediction;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMyMediaLiteRatingPredictionRecSource<TRecommender>
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, RatingPredictionRecommendation>
        where TRecommender : RatingPredictor, IFoldInRatingPredictor
    {
        private MyMediaLiteRatingPredictionRecSource<TRecommender> m_recommender;
        private int m_minEpisodesToCountIncomplete;
        private bool m_useDropped;

        public MalMyMediaLiteRatingPredictionRecSource(TRecommender recommender, int minEpisodesToCountIncomplete, bool useDropped)
        {
            m_recommender = new MyMediaLiteRatingPredictionRecSource<TRecommender>(recommender);
            m_minEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            m_useDropped = useDropped;
        }

        public void Train(MalTrainingData trainingData)
        {
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(m_minEpisodesToCountIncomplete, m_useDropped);

            m_recommender.Train(basicTrainingData);
        }

        public IEnumerable<RatingPredictionRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            IBasicInputForUser basicInput = inputForUser.AsBasicInput(m_minEpisodesToCountIncomplete, m_useDropped);
            return m_recommender.GetRecommendations(basicInput, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("{0} MinEpisodesToCountIncomplete = {1}, UseDropped = {2}",
                m_recommender, m_minEpisodesToCountIncomplete, m_useDropped);
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