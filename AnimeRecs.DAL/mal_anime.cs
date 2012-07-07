using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.DAL
{
    public class mal_anime
    {
        public int mal_anime_id { get; set; }
        public string title { get; set; }
        public int mal_anime_type_id { get; set; }

        /// <summary>
        /// Time anime was last updated by AnimeRecs, not by MAL
        /// </summary>
        public DateTime last_updated { get; set; }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.DAL.
//
// AnimeRecs.DAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.DAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.DAL.  If not, see <http://www.gnu.org/licenses/>.