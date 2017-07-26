using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using System.Threading;
using System.Threading.Tasks;

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
            conn.Execute(sql, new { MalUserId = mal_user_id, MalName = mal_name, TimeAdded = time_added }, transaction);
        }

        public static async Task<IList<mal_user>> GetAllAsync(NpgsqlConnection conn, NpgsqlTransaction transaction, CancellationToken cancellationToken)
        {
            string sql = @"
SELECT mal_user_id, mal_name, time_added
FROM mal_user
";
            TimeSpan timeout = TimeSpan.FromSeconds(10); // TODO: make this configurable

            try
            {
                return await conn.QueryAsyncWithCancellation<mal_user>(sql, timeout, cancellationToken, transaction).ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error loading all MAL users from database: {0}", ex.Message), ex);
            }
        }

        public static bool UserIsInDbCaseSensitive(string username, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            long count = conn.Query<long>(@"SELECT Count(*) FROM mal_user WHERE mal_name = :Username", new { Username = username }, transaction).First();
            return count > 0;
        }

        public static bool UserIsInDb(int userId, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            long count = conn.Query<long>(@"SELECT Count(*) FROM mal_user WHERE mal_user_id = :UserId", new { UserId = userId }, transaction).First();
            return count > 0;
        }

        public static long Count(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            long count = conn.Query<long>("SELECT Count(*) FROM mal_user", transaction: transaction).First();
            return count;
        }

        public static void DeleteOldestUsers(long numUsers, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            if (numUsers == 0)
            {
                return;
            }
            if (numUsers < 0)
            {
                throw new ArgumentOutOfRangeException("numUsers", numUsers, string.Format("Cannot delete {0} oldest users", numUsers));
            }

            string deleteSql = @"DELETE FROM mal_user WHERE mal_user_id IN
(SELECT mal_user_id FROM mal_user
ORDER BY time_added
LIMIT :NumToDelete)";

            conn.Execute(deleteSql, new { NumToDelete = numUsers }, transaction);
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