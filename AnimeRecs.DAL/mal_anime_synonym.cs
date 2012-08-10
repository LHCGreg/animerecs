using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;
using System.Globalization;
using MiscUtil.Collections.Extensions;

namespace AnimeRecs.DAL
{
    public class mal_anime_synonym
    {
        public int mal_anime_synonym_id { get; set; }
        public int mal_anime_id { get; set; }
        public string synonym { get; set; }

        public mal_anime_synonym()
        {
            ;
        }

        public mal_anime_synonym(int _mal_anime_id, string _synonym)
        {
            mal_anime_id = _mal_anime_id;
            synonym = _synonym;
        }

        public static int Delete(IEnumerable<int> malAnimeIds, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            string idList = string.Join(", ", malAnimeIds.Select(id => id.ToString(CultureInfo.InvariantCulture)));
            string deleteSql = string.Format("DELETE FROM mal_anime_synonym WHERE mal_anime_id IN ({0})", idList);
            int numRowsDeleted = conn.Execute(deleteSql, transaction: transaction);
            return numRowsDeleted;
        }

        public static int Insert(IEnumerable<mal_anime_synonym> synonyms, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            StringBuilder sqlBuilder = new StringBuilder("INSERT INTO mal_anime_synonym (mal_anime_id, synonym) VALUES ");

            bool anyRows = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                int numSynonym = 1;
                foreach (var synIter in synonyms.AsSmartEnumerable())
                {
                    mal_anime_synonym synonym = synIter.Value;
                    if (!synIter.IsFirst)
                    {
                        sqlBuilder.AppendLine(", ");
                    }
                    sqlBuilder.AppendFormat("(:MalAnimeId{0}, :Synonym{0})", numSynonym);

                    cmd.Parameters.AddWithValue(string.Format("MalAnimeId{0}", numSynonym), synonym.mal_anime_id);
                    cmd.Parameters.AddWithValue(string.Format("Synonym{0}", numSynonym), synonym.synonym);

                    numSynonym++;
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