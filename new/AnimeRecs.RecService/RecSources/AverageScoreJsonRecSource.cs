using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.RecSources
{
    internal class AverageScoreJsonRecSource : ITrainableJsonRecSource
    {
        private MalAverageScoreRecSource m_underlyingRecSource;
        private IDictionary<int, MalAnime> m_animesAvailableForRecommendation = new Dictionary<int, MalAnime>();

        public AverageScoreJsonRecSource(int minEpisodesToCountIncomplete, bool useDropped, int minUsersToCountAnime)
        {
            m_underlyingRecSource = new MalAverageScoreRecSource(minEpisodesToCountIncomplete, useDropped, minUsersToCountAnime);
        }

        public void Train(MalTrainingData trainingData)
        {
            m_underlyingRecSource.Train(trainingData);
            m_animesAvailableForRecommendation = trainingData.Animes;
        }

        public GetMalRecsResponse GetRecommendations(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            List<RecEngine.AverageScoreRecommendation> recs = m_underlyingRecSource.GetRecommendations(animeList, recRequest.NumRecsDesired).ToList();

            List<DTO.AverageScoreRecommendation> dtoRecs = recs.Select(engineRec => new DTO.AverageScoreRecommendation(
                malAnimeId: engineRec.ItemId,
                title: m_animesAvailableForRecommendation[engineRec.ItemId].Title,
                malAnimeType: m_animesAvailableForRecommendation[engineRec.ItemId].Type,
                numRatings: engineRec.NumRatings,
                averageScore: engineRec.AverageScore
            )).ToList();

            GetMalRecsResponse<DTO.AverageScoreRecommendation> response =
                new GetMalRecsResponse<DTO.AverageScoreRecommendation>(
                    recommendationType: RecommendationTypes.AverageScore, recommendations: dtoRecs);

            return response;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.