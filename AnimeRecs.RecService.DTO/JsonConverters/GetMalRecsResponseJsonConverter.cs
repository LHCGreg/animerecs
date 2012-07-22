using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class GetMalRecsResponseJsonConverter : JsonCreationConverter<GetMalRecsResponse>
    {
        protected override GetMalRecsResponse Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            string recType = jObject.Value<string>("RecommendationType");

            if (recType.Equals(RecommendationTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
                return new GetMalRecsResponse<AverageScoreRecommendation>();
            else if (recType.Equals(RecommendationTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
                return new GetMalRecsResponse<MostPopularRecommendation>();
            else if (recType.Equals(RecommendationTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
                return new GetMalRecsResponse<AnimeRecsRecommendation, MalAnimeRecsExtraResponseData>();
            else if (recType.Equals(RecommendationTypes.RatingPrediction, StringComparison.OrdinalIgnoreCase))
                return new GetMalRecsResponse<RatingPredictionRecommendation>();
            else
                return new GetMalRecsResponse<Recommendation>();
        }
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