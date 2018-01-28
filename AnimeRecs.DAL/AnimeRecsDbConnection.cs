using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using Dapper;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.DAL
{
    public class AnimeRecsDbConnection : IAnimeRecsDbConnection
    {
        private NpgsqlConnection Conn { get; set; }
        
        public AnimeRecsDbConnection(string pgConnectionString)
        {
            Conn = new NpgsqlConnection(pgConnectionString);
        }

        public Task OpenAsync(CancellationToken cancellationToken)
        {
            return Conn.OpenAsync(cancellationToken);
        }

        public async Task<IDictionary<int, ICollection<streaming_service_anime_map>>> GetStreamsAsync(IEnumerable<int> malAnimeIds, CancellationToken cancellationToken)
        {
            if (!malAnimeIds.Any())
            {
                return new Dictionary<int, ICollection<streaming_service_anime_map>>();
            }

            Dictionary<int, ICollection<streaming_service_anime_map>> streamsByAnime = new Dictionary<int, ICollection<streaming_service_anime_map>>();

            string malAnimeIdList = string.Join(", ", malAnimeIds);

            string sql = string.Format(@"SELECT * FROM streaming_service_anime_map WHERE mal_anime_id IN ({0})", malAnimeIdList);
            foreach (streaming_service_anime_map map in await Conn.QueryAsyncWithCancellation<streaming_service_anime_map>(sql, cancellationToken).ConfigureAwait(false))
            {
                if (!streamsByAnime.ContainsKey(map.mal_anime_id))
                {
                    streamsByAnime[map.mal_anime_id] = new List<streaming_service_anime_map>();
                }
                streamsByAnime[map.mal_anime_id].Add(map);
            }

            return streamsByAnime;
        }

        public void Dispose()
        {
            Conn.Dispose();
        }
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