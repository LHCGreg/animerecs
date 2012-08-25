using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Not specified means a blank in the csv. Specified but null MalId means n/a.
    /// </summary>
    struct MalId
    {
        public bool Specified { get; private set; }
        public int? MalAnimeId { get; private set; }

        public MalId(int? malAnimeId, bool specified)
            : this()
        {
            MalAnimeId = malAnimeId;
            Specified = specified;
        }

        public override string ToString()
        {
            if (MalAnimeId != null)
            {
                return MalAnimeId.Value.ToString(CultureInfo.InvariantCulture);
            }
            else if (Specified)
            {
                return "n/a";
            }
            else
            {
                return "";
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.