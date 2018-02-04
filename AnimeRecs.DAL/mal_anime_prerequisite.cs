using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Npgsql;
using Dapper;
using System.Threading.Tasks;
using System.Threading;

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

        public static async Task<IList<mal_anime_prerequisite>> GetAllAsync(NpgsqlConnection conn, NpgsqlTransaction transaction, CancellationToken cancellationToken)
        {
            string sql = "SELECT * FROM mal_anime_prerequisite";
            TimeSpan timeout = TimeSpan.FromSeconds(10); // TODO: Make this configurable

            try
            {
                return await conn.QueryAsyncWithCancellation<mal_anime_prerequisite>(sql, timeout, cancellationToken, transaction).ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error loading all MAL anime prerequisites from database: {0}", ex.Message), ex);
            }
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

            bool first = true;
            foreach (var prereqMap in prereqMaps)
            {
                if (!first)
                {
                    sql.AppendLine(",");
                }
                sql.AppendFormat("({0}, {1})",
                    prereqMap.mal_anime_id.ToString(CultureInfo.InvariantCulture),
                    prereqMap.prerequisite_mal_anime_id.ToString(CultureInfo.InvariantCulture)
                );

                first = false;
            }
            sql.AppendLine(";");
            sql.AppendLine();
            sql.Append("COMMIT TRANSACTION;");

            return sql.ToString();
        }
    }
}
