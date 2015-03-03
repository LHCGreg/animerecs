using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace AnimeRecs.Web.Modules.GetRecs
{
    public class AnimeRecsInputJson
    {
        [Required]
        public string MalName { get; set; }

        private IList<int> _animeIdsToWithhold = new List<int>(0);
        /// <summary>
        /// Ids of anime to remove from the user's anime list before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source. If I remove my absolute favorite anime, does the
        /// rec source put it high in the recommendations?
        /// </summary>
        public IList<int> AnimeIdsToWithhold
        {
            get
            {
                return _animeIdsToWithhold;
            }
            set
            {
                // Make this property not nullable by treating setting to null as setting to empty list.
                if (value == null)
                {
                    _animeIdsToWithhold = new List<int>(0);
                }
                else
                {
                    _animeIdsToWithhold = value;
                }
            }
        }

        /// <summary>
        /// If non-zero, removes the given percentage of the user's anime list randomly before passing it to the recommendation engine.
        /// This is useful for subjectively evaluating a rec source.
        /// </summary>
        public decimal? PercentOfAnimeToWithhold { get; set; }

        // If both GoodCutoff and GoodPercentile are null, use a default

        public decimal? GoodCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }

        // Only applicable for AnimeRecs...should think about how to take parameter specific to a rec source type.
        public bool DisplayDetailedResults { get; set; }

        [Required]
        public string RecSourceName { get; set; }

        public AnimeRecsInputJson()
        {
            AnimeIdsToWithhold = new List<int>();
        }

        public override string ToString()
        {
            return MalName;
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.