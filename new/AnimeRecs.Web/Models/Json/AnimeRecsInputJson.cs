using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimeRecs.Web.Models.Json
{
    public class AnimeRecsInputJson
    {
        [Required]
        public string MalName { get; set; }

        // If both GoodCutoff and GoodPercentile are null, use a default
        
        public decimal? GoodCutoff { get; set; }

        [Range(0, 100)]
        public decimal? GoodPercentile { get; set; }

        public string RecSourceName { get; set; }

        public override string ToString()
        {
            return MalName;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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