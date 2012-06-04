using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class RecSourceParams
    {

    }

    public class AverageScoreRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public int MinUsersToCountAnime { get; set; }
        public bool UseDropped { get; set; }

        public AverageScoreRecSourceParams()
        {
            ;
        }

        public AverageScoreRecSourceParams(int minEpisodesToCountIncomplete, int minUsersToCountAnime, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            MinUsersToCountAnime = minUsersToCountAnime;
            UseDropped = useDropped;
        }
    }

    public class MostPopularRecSourceParams : RecSourceParams
    {
        public int MinEpisodesToCountIncomplete { get; set; }
        public bool UseDropped { get; set; }

        public MostPopularRecSourceParams()
        {
            ;
        }

        public MostPopularRecSourceParams(int minEpisodesToCountIncomplete, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
        }
    }

    public class AnimeRecsRecSourceParams : RecSourceParams
    {
        public int NumRecommendersToUse { get; set; }
        public double FractionConsideredRecommended { get; set; }
        public int MinEpisodesToClassifyIncomplete { get; set; }

        public AnimeRecsRecSourceParams()
        {
            ;
        }

        public AnimeRecsRecSourceParams(int numRecommendersToUse, double fractionConsideredRecommended, int minEpisodesToClassifyIncomplete)
        {
            NumRecommendersToUse = numRecommendersToUse;
            FractionConsideredRecommended = fractionConsideredRecommended;
            MinEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
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