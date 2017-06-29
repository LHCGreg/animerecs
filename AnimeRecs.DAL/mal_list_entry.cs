using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace AnimeRecs.DAL
{
    public class mal_list_entry
    {
        public int mal_list_entry_id { get; set; }
        public int mal_user_id { get; set; }
        public int mal_anime_id { get; set; }
        public short? rating { get; set; }
        public short mal_list_entry_status_id { get; set; }
        public short num_episodes_watched { get; set; }
        public short? started_watching_year { get; set; }
        public short? started_watching_month { get; set; }
        public short? started_watching_day { get; set; }
        public short? finished_watching_year { get; set; }
        public short? finished_watching_month { get; set; }
        public short? finished_watching_day { get; set; }
        public DateTime last_mal_update { get; set; }

        public mal_list_entry()
        {
            ;
        }

        public mal_list_entry(int _mal_user_id, int _mal_anime_id, short? _rating, short _mal_list_entry_status_id,
            short _num_episodes_watched, short? _started_watching_year, short? _started_watching_month, short? _started_watching_day,
            short? _finished_watching_year, short? _finished_watching_month, short? _finished_watching_day, DateTime _last_mal_update)
        {
            mal_user_id = _mal_user_id;
            mal_anime_id = _mal_anime_id;
            rating = _rating;
            mal_list_entry_status_id = _mal_list_entry_status_id;
            num_episodes_watched = _num_episodes_watched;
            started_watching_year = _started_watching_year;
            started_watching_month = _started_watching_month;
            started_watching_day = _started_watching_day;
            finished_watching_year = _finished_watching_year;
            finished_watching_month = _finished_watching_month;
            finished_watching_day = _finished_watching_day;
            last_mal_update = _last_mal_update;
        }

        public static int Insert(IEnumerable<mal_list_entry> entries, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            StringBuilder sqlBuilder = new StringBuilder(@"INSERT INTO mal_list_entry
(mal_user_id, mal_anime_id, rating, mal_list_entry_status_id, num_episodes_watched,
started_watching_year, started_watching_month, started_watching_day,
finished_watching_year, finished_watching_month, finished_watching_day, last_mal_update)

VALUES
");

            bool anyRows = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                int numEntry = 1;
                foreach (mal_list_entry entry in entries)
                {
                    if (numEntry > 1)
                    {
                        sqlBuilder.AppendLine(", ");
                    }
                    sqlBuilder.AppendFormat(@"(:MalUserId{0}, :MalAnimeId{0}, :Rating{0}, :MalListEntryStatusId{0},
:NumEpisodesWatched{0}, :StartedWatchingYear{0}, :StartedWatchingMonth{0}, :StartedWatchingDay{0},
:FinishedWatchingYear{0}, :FinishedWatchingMonth{0}, :FinishedWatchingDay{0}, :LastMalUpdate{0})", numEntry);

                    cmd.Parameters.AddWithValue(string.Format("MalUserId{0}", numEntry), entry.mal_user_id);
                    cmd.Parameters.AddWithValue(string.Format("MalAnimeId{0}", numEntry), entry.mal_anime_id);
                    cmd.Parameters.AddWithValue(string.Format("Rating{0}", numEntry), (object)entry.rating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("MalListEntryStatusId{0}", numEntry), entry.mal_list_entry_status_id);
                    cmd.Parameters.AddWithValue(string.Format("NumEpisodesWatched{0}", numEntry), entry.num_episodes_watched);
                    cmd.Parameters.AddWithValue(string.Format("StartedWatchingYear{0}", numEntry), (object)entry.started_watching_year ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("StartedWatchingMonth{0}", numEntry), (object)entry.started_watching_month ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("StartedWatchingDay{0}", numEntry), (object)entry.started_watching_day ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("FinishedWatchingYear{0}", numEntry), (object)entry.finished_watching_year ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("FinishedWatchingMonth{0}", numEntry), (object)entry.finished_watching_month ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("FinishedWatchingDay{0}", numEntry), (object)entry.finished_watching_day ?? DBNull.Value);
                    cmd.Parameters.AddWithValue(string.Format("LastMalUpdate{0}", numEntry), entry.last_mal_update);

                    numEntry++;
                    anyRows = true;
                }

                if (anyRows)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandText = sqlBuilder.ToString();
                    int numRowsInserted = cmd.ExecuteNonQuery();
                    return numRowsInserted;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}

// Copyright (C) 2017 Greg Najda
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