using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;

namespace AnimeRecs.DAL
{
    public class mal_user
    {
        public int mal_user_id { get; set; }
        public string mal_name { get; set; }
        public DateTime time_added { get; set; }

        public mal_user()
        {
            ;
        }

        public mal_user(int _mal_user_id, string _mal_name, DateTime _time_added)
        {
            mal_user_id = _mal_user_id;
            mal_name = _mal_name;
            time_added = _time_added;
        }

        public void Insert(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = "INSERT INTO mal_user (mal_user_id, mal_name, time_added) VALUES (:MalUserId, :MalName, :TimeAdded)";
            int rowsAffected = conn.Execute(sql, new { MalUserId = mal_user_id, MalName = mal_name, TimeAdded = time_added }, transaction);
        }

        public static IEnumerable<mal_user> GetAll(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = @"
SELECT mal_user_id, mal_name, time_added
FROM mal_user
";
            // This will buffer all rows in memory before returning
            return conn.Query<mal_user>(sql, transaction: transaction);
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