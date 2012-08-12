using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class RecommendationTypes
    {
        public static string BasicRecommendation { get { return "BasicRecommendation"; } }
        public static string AverageScore { get { return "AverageScore"; } }
        public static string MostPopular { get { return "MostPopular"; } }
        public static string AnimeRecs { get { return "AnimeRecs"; } }
        public static string RatingPrediction { get { return "RatingPrediction"; } }

        // Will fall back to a basic Recomendation is none of these recomendation types match
        private static IDictionary<string, Func<GetMalRecsResponse>> s_getMalRecsResponseFactories =
            new Dictionary<string, Func<GetMalRecsResponse>>(StringComparer.OrdinalIgnoreCase)
            {
                { AverageScore, () => new GetMalRecsResponse<AverageScoreRecommendation>() },
                { MostPopular, () => new GetMalRecsResponse<MostPopularRecommendation>() },
                { AnimeRecs, () => new GetMalRecsResponse<AnimeRecsRecommendation, MalAnimeRecsExtraResponseData>() },
                { RatingPrediction, () => new GetMalRecsResponse<RatingPredictionRecommendation>() }
            };
        public static IDictionary<string, Func<GetMalRecsResponse>> GetMalRecsResponseFactories { get { return s_getMalRecsResponseFactories; } }

    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.