using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Dapper;

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
                foreach (mal_list_entry_tag tag in tags)
                {
                    if (numTag > 1)
                    {
                        sqlBuilder.AppendLine(", ");
                    }

                    sqlBuilder.AppendFormat("(:MalUserId{0}, :MalAnimeId{0}, :Tag{0})", numTag);

                    cmd.Parameters.AddWithValue(string.Format("MalUserId{0}", numTag), tag.mal_user_id);
                    cmd.Parameters.AddWithValue(string.Format("MalAnimeId{0}", numTag), tag.mal_anime_id);
                    cmd.Parameters.AddWithValue(string.Format("Tag{0}", numTag), (object)tag.tag ?? DBNull.Value);

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
