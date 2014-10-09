using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AnimeRecs.NancyWeb.Modules.GetRecs
{
    public class AnimeRecsInputJson
    {
        [Required]
        public string MalName { get; set; }

        /// <summary>
        /// Ids of anime to remove from the user's anime list before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source. If I remove my absolute favorite anime, does the
        /// rec source put it high in the recommendations?
        /// </summary>
        public IList<int> AnimeIdsToWithhold { get; set; }

        /// <summary>
        /// If non-zero, removes the given percentage of the user's anime list randomly before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source.
        /// </summary>
        public decimal PercentOfAnimeToWithhold { get; set; }

        // If both GoodCutoff and GoodPercentile are null, use a default

        public decimal? GoodCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }

        // Only applicable for AnimeRecs...should think about how to take parameter specific to a rec source type.
        public bool DisplayDetailedResults { get; set; }

        public string RecSourceName { get; set; }

        public AnimeRecsInputJson()
        {
            AnimeIdsToWithhold = new List<int>();
            PercentOfAnimeToWithhold = 0m;
        }

        public override string ToString()
        {
            return MalName;
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.NancyWeb.
//
// AnimeRecs.NancyWeb is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.NancyWeb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.NancyWeb.  If not, see <http://www.gnu.org/licenses/>.