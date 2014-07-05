using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class GetMalRecsRequest
    {
        public MalListForUser AnimeList { get; set; }
        public string RecSourceName { get; set; }
        public int NumRecsDesired { get; set; }

        // XXX: Move these to rec source-specific DTOs
        public decimal? TargetScore { get; set; }
        public decimal? TargetFraction { get; set; }

        public GetMalRecsRequest()
        {
            ;
        }

        public static GetMalRecsRequest CreateWithTargetScore(string recSourceName, int numRecsDesired, decimal targetScore, MalListForUser animeList)
        {
            return new GetMalRecsRequest()
            {
                RecSourceName = recSourceName,
                NumRecsDesired = numRecsDesired,
                TargetScore = targetScore,
                AnimeList = animeList
            };
        }

        public static GetMalRecsRequest CreateWithTargetFraction(string recSourceName, int numRecsDesired, decimal targetFraction, MalListForUser animeList)
        {
            return new GetMalRecsRequest()
            {
                RecSourceName = recSourceName,
                NumRecsDesired = numRecsDesired,
                TargetFraction = targetFraction,
                AnimeList = animeList
            };
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