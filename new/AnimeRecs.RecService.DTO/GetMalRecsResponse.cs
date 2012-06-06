using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class GetMalRecsResponse : Response
    {
        public string RecommendationType { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType)
        {
            RecommendationType = recommendationType;
        }
    }
    
    public class GetMalRecsResponse<TRecommendation> : GetMalRecsResponse
        where TRecommendation : Recommendation
    {
        public IList<TRecommendation> Recommendations { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<TRecommendation> recommendations)
            : base(recommendationType)
        {
            Recommendations = recommendations;
        }
    }

    public class GetMalRecsResponse<TRecommendation, TData> : GetMalRecsResponse<TRecommendation>
        where TRecommendation : Recommendation
    {
        public TData Data { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<TRecommendation> recommendations, TData data)
            : base(recommendationType, recommendations)
        {
            Data = data;
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