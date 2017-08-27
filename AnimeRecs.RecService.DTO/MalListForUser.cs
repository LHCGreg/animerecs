using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalListForUser
    {
        public IList<MalListEntry> Entries { get; set; }

        public MalListForUser()
        {
            ;
        }

        public MalListForUser(IList<MalListEntry> entries)
        {
            Entries = entries;
        }

        public override string ToString()
        {
            return string.Format("{0} entries", Entries.Count);
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