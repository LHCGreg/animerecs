using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;

namespace AnimeRecs.DAL
{
    public class mal_anime
    {
        public int mal_anime_id { get; set; }
        public string title { get; set; }
        public int mal_anime_type_id { get; set; }
        public int num_episodes { get; set; }
        public int mal_anime_status_id { get; set; }
        public short? start_year { get; set; }
        public short? start_month { get; set; }
        public short? start_day { get; set; }
        public short? end_year { get; set; }
        public short? end_month { get; set; }
        public short? end_day { get; set; }
        public string image_url { get; set; }

        /// <summary>
        /// Time anime was last updated by AnimeRecs, not by MAL
        /// </summary>
        public DateTime last_updated { get; set; }

        public mal_anime()
        {
            ;
        }

        public mal_anime(int _mal_anime_id, string _title, int _mal_anime_type_id, int _num_episodes, int _mal_anime_status_id,
            short? _start_year, short? _start_month, short? _start_day, short? _end_year, short? _end_month, short? _end_day, string _image_url,
            DateTime _last_updated)
        {
            mal_anime_id = _mal_anime_id;
            title = _title;
            mal_anime_type_id = _mal_anime_type_id;
            num_episodes = _num_episodes;
            mal_anime_status_id = _mal_anime_status_id;
            start_year = _start_year;
            start_month = _start_month;
            start_day = _start_day;
            end_year = _end_year;
            end_month = _end_month;
            end_day = _end_day;
            image_url = _image_url;
            last_updated = _last_updated;
        }

        public void Insert(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = @"INSERT INTO mal_anime
(mal_anime_id, title, mal_anime_type_id, num_episodes, mal_anime_status_id, start_year, start_month, start_day,
end_year, end_month, end_day, image_url, last_updated)
VALUES
(:MalAnimeId, :Title, :MalAnimeTypeId, :NumEpisodes, :MalAnimeStatusId, :StartYear, :StartMonth, :StartDay,
:EndYear, :EndMonth, :EndDay, :ImageUrl, :LastUpdated)";

            conn.Execute(sql,
                new
                {
                    MalAnimeId = mal_anime_id,
                    Title = title,
                    MalAnimeTypeId = mal_anime_type_id,
                    NumEpisodes = num_episodes,
                    MalAnimeStatusId = mal_anime_status_id,
                    StartYear = start_year,
                    StartMonth = start_month,
                    StartDay = start_day,
                    EndYear = end_year,
                    EndMonth = end_month,
                    EndDay = end_day,
                    ImageUrl = image_url,
                    LastUpdated = last_updated
                },

                transaction);
        }

        public void Update(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = @"UPDATE mal_anime
SET title = :Title, mal_anime_type_id = :MalAnimeTypeID, num_episodes = :NumEpisodes,
mal_anime_status_id = :MalAnimeStatusId, start_year = :StartYear, start_month = :StartMonth, start_day = :StartDay,
end_year = :EndYear, end_month = :EndMonth, end_day = :EndDay, image_url = :ImageUrl, last_updated = :LastUpdated

WHERE mal_anime_id = :MalAnimeId";

            conn.Execute(sql,
                new
                {
                    MalAnimeId = mal_anime_id,
                    Title = title,
                    MalAnimeTypeId = mal_anime_type_id,
                    NumEpisodes = num_episodes,
                    MalAnimeStatusId = mal_anime_status_id,
                    StartYear = start_year,
                    StartMonth = start_month,
                    StartDay = start_day,
                    EndYear = end_year,
                    EndMonth = end_month,
                    EndDay = end_day,
                    ImageUrl = image_url,
                    LastUpdated = last_updated
                },

                transaction);
        }

        public static IEnumerable<mal_anime> GetAll(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = @"
SELECT mal_anime_id, title, mal_anime_type_id, num_episodes, mal_anime_status_id, start_year, start_month, start_day,
end_year, end_month, end_day, image_url, last_updated
FROM mal_anime
";

            // This buffers all the rows in memory before returning
            return conn.Query<mal_anime>(sql, transaction: transaction);
        }

        public static bool IsInDatabase(int animeId, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            long count = conn.Query<long>("SELECT Count(*) FROM mal_anime WHERE mal_anime_id = :AnimeId",
                    new { AnimeId = animeId }, transaction).First();
            return count > 0;
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