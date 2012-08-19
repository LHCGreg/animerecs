using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AnimeRecs.DAL
{
    public interface IAnimeRecsDbConnection : IDisposable
    {
        IDbConnection Conn { get; }

        IDictionary<int, ICollection<streaming_service_anime_map>> GetStreams(IEnumerable<int> malAnimeIds);
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