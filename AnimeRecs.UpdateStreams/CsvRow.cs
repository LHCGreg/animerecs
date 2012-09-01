using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class CsvRow
    {
        public StreamingService Service { get; private set; }
        public string AnimeName { get; private set; }
        public string Url { get; private set; }
        public MalId MalAnimeId { get; private set; }

        public CsvRow(StreamingService service, string animeName, string url, MalId malAnimeId)
        {
            Service = service;
            AnimeName = animeName;
            Url = url;
            MalAnimeId = malAnimeId;
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