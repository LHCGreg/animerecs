using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using MiscUtil.Collections.Extensions;

namespace AnimeRecs.DAL
{
    public class mal_list_entry_tag
    {
        public int mal_list_entry_tag_id { get; set; }
        public int mal_user_id { get; set; }
        public int mal_anime_id { get; set; }
        public string tag { get; set; }

        public mal_list_entry_tag()
        {
            ;
        }

        public mal_list_entry_tag(int _mal_list_entry_tag_id, int _mal_user_id, int _mal_anime_id, string _tag)
        {
            mal_list_entry_tag_id = _mal_list_entry_tag_id;
            mal_user_id = _mal_user_id;
            mal_anime_id = _mal_anime_id;
            tag = _tag;
        }

        public mal_list_entry_tag(int _mal_user_id, int _mal_anime_id, string _tag)
        {
            mal_user_id = _mal_user_id;
            mal_anime_id = _mal_anime_id;
            tag = _tag;
        }

        public static int Insert(IEnumerable<mal_list_entry_tag> tags, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            StringBuilder sqlBuilder = new StringBuilder(@"INSERT INTO mal_list_entry_tag
(mal_user_id, mal_anime_id, tag)
VALUES
");

            bool anyRows = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                int numTag = 1;
                foreach (var tagIter in tags.AsSmartEnumerable())
                {
                    mal_list_entry_tag tag = tagIter.Value;
                    if (!tagIter.IsFirst)
                    {
                        sqlBuilder.AppendLine(", ");
                    }

                    sqlBuilder.AppendFormat("(:MalUserId{0}, :MalAnimeId{0}, :Tag{0})", numTag);

                    cmd.Parameters.AddWithValue(string.Format("MalUserId{0}", numTag), tag.mal_user_id);
                    cmd.Parameters.AddWithValue(string.Format("MalAnimeId{0}", numTag), tag.mal_anime_id);
                    cmd.Parameters.AddWithValue(string.Format("Tag{0}", numTag), tag.tag);

                    numTag++;
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