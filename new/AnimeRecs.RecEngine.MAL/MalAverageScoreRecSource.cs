using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalAverageScoreRecSource : ITrainableRecSource<MalTrainingData, MalUserListEntries, AverageScoreRecommendation>
    {
        private AverageScoreRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>
            m_recommender = new AverageScoreRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>();

        private int m_minEpisodesToCountIncomplete;
        private bool m_useDropped;
        private int m_minUsersToCountAnime;
        
        public MalAverageScoreRecSource(int minEpisodesToCountIncomplete, bool useDropped, int minUsersToCountAnime)
        {
            m_minEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            m_useDropped = useDropped;
            m_minUsersToCountAnime = minUsersToCountAnime;
        }

        public void Train(MalTrainingData trainingData)
        {
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(m_minEpisodesToCountIncomplete, m_useDropped);

            IBasicTrainingData<IBasicInputForUser> filteredTrainingData = FilterHelpers.RemoveItemsWithFewUsers(basicTrainingData, m_minUsersToCountAnime);
            m_recommender.Train(filteredTrainingData);
        }

        public IEnumerable<AverageScoreRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_recommender.GetRecommendations(inputForUser, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("Average - MinEpisodesToCountIncomplete = {0}, UseDropped = {1}, MinUsersToCountAnime = {2}",
                m_minEpisodesToCountIncomplete, m_useDropped, m_minUsersToCountAnime);
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