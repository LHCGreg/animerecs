using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AnimeRecs.DAL
{
    public class streaming_service_anime_map
    {
        public int streaming_service_anime_map_id { get; set; }
        public int mal_anime_id { get; set; }
        public int streaming_service_id { get; set; }
        public string streaming_url { get; set; }

        public streaming_service_anime_map()
        {
            ;
        }

        public streaming_service_anime_map(int _mal_anime_id, int _streaming_service_id, string _streaming_url)
        {
            mal_anime_id = _mal_anime_id;
            streaming_service_id = _streaming_service_id;
            streaming_url = _streaming_url;
        }

        public static string CreateRefreshStreamMapSql(IEnumerable<streaming_service_anime_map> streamMaps)
        {
            if (!streamMaps.Any())
            {
                return "";
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("BEGIN TRANSACTION;");
            sql.AppendLine();
            sql.AppendLine("DELETE FROM streaming_service_anime_map WHERE 1 = 1;");
            sql.AppendLine();
            sql.AppendLine("INSERT INTO streaming_service_anime_map");
            sql.AppendLine("(mal_anime_id, streaming_service_id, streaming_url)");
            sql.AppendLine("VALUES");

            bool first = true;
            foreach (streaming_service_anime_map streamMap in streamMaps)
            {
                if (!first)
                {
                    sql.AppendLine(",");
                }
                sql.AppendFormat("({0}, {1}, {2})",
                    streamMap.mal_anime_id.ToString(CultureInfo.InvariantCulture),
                    streamMap.streaming_service_id.ToString(CultureInfo.InvariantCulture),
                    PgHelpers.QuotePgString(streamMap.streaming_url)
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
