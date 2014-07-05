using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMostPopularRecSource
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<MostPopularRecommendation>, MostPopularRecommendation>
    {
        private MostPopularRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>
            m_recommender = new MostPopularRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>();

        public int MinEpisodesToCountIncomplete { get; private set; }
        public bool UseDropped { get; private set; }

        public MalMostPopularRecSource(int minEpisodesToCountIncomplete, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
        }

        public void Train(MalTrainingData trainingData)
        {
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(MinEpisodesToCountIncomplete, UseDropped);
            m_recommender.Train(basicTrainingData);
        }

        public IEnumerable<MostPopularRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_recommender.GetRecommendations(inputForUser, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("MostPopular MinEpisodesToCountIncomplete={0}, UseDropped={1}",
                MinEpisodesToCountIncomplete, UseDropped);
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