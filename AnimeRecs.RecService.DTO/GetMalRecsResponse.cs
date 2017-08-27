using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO.JsonConverters;

namespace AnimeRecs.RecService.DTO
{
    [JsonConverter(typeof(GetMalRecsResponseJsonConverter))]
    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse
    {
        public string RecommendationType { get; set; }
        public IList<DTO.MalAnime> Animes { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes)
        {
            RecommendationType = recommendationType;
            Animes = animes;
        }
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse<TRecommendation> : GetMalRecsResponse
        where TRecommendation : Recommendation
    {
        public IList<TRecommendation> Recommendations { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes, IList<TRecommendation> recommendations)
            : base(recommendationType, animes)
        {
            Recommendations = recommendations;
        }
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse<TRecommendation, TData> : GetMalRecsResponse<TRecommendation>
        where TRecommendation : Recommendation
    {
        public TData Data { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes, IList<TRecommendation> recommendations, TData data)
            : base(recommendationType, animes, recommendations)
        {
            Data = data;
        }
    }
}

// Copyright (C) 2017 Greg Najda
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