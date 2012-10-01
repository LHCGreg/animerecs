using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Npgsql;
using Dapper;
using MiscUtil.Collections.Extensions;

namespace AnimeRecs.DAL
{
    public class mal_anime_prerequisite
    {
        public int mal_anime_prerequisite_id { get; set; }
        public int mal_anime_id { get; set; }
        public int prerequisite_mal_anime_id { get; set; }

        public mal_anime_prerequisite()
        {
            ;
        }

        public mal_anime_prerequisite(int _mal_anime_id, int _prerequisite_mal_anime_id)
        {
            mal_anime_id = _mal_anime_id;
            prerequisite_mal_anime_id = _prerequisite_mal_anime_id;
        }

        public static IEnumerable<mal_anime_prerequisite> GetAll(NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string sql = "SELECT * FROM mal_anime_prerequisite";
            return conn.Query<mal_anime_prerequisite>(sql, transaction: transaction);
        }

        public static string CreateRefreshPrerequisiteMapSql(IEnumerable<mal_anime_prerequisite> prereqMaps)
        {
            if (!prereqMaps.Any())
            {
                return "";
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("BEGIN TRANSACTION;");
            sql.AppendLine();
            sql.AppendLine("DELETE FROM mal_anime_prerequisite WHERE 1 = 1;");
            sql.AppendLine();
            sql.AppendLine("INSERT INTO mal_anime_prerequisite");
            sql.AppendLine("(mal_anime_id, prerequisite_mal_anime_id)");
            sql.AppendLine("VALUES");

            foreach (var prereqMapIt in prereqMaps.AsSmartEnumerable())
            {
                mal_anime_prerequisite prereqMap = prereqMapIt.Value;
                if (!prereqMapIt.IsFirst)
                {
                    sql.AppendLine(",");
                }
                sql.AppendFormat("({0}, {1})",
                    prereqMap.mal_anime_id.ToString(CultureInfo.InvariantCulture),
                    prereqMap.prerequisite_mal_anime_id.ToString(CultureInfo.InvariantCulture)
                );
            }
            sql.AppendLine(";");
            sql.AppendLine();
            sql.Append("COMMIT TRANSACTION;");

            return sql.ToString();
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