using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Collections.Extensions;
using System.Globalization;

namespace AnimeRecs.DAL
{
    public class streaming_service_anime_map
    {
        public int streaming_service_anime_map_id { get; set; }
        public int mal_anime_id { get; set; }
        public int streaming_service_id { get; set; }
        public bool requires_subscription { get; set; }
        public string streaming_url { get; set; }

        public streaming_service_anime_map()
        {
            ;
        }

        public streaming_service_anime_map(int _mal_anime_id, int _streaming_service_id, bool _requires_subscription, string _streaming_url)
        {
            mal_anime_id = _mal_anime_id;
            streaming_service_id = _streaming_service_id;
            requires_subscription = _requires_subscription;
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
            sql.AppendLine("(mal_anime_id, streaming_service_id, streaming_url, requires_subscription)");
            sql.AppendLine("VALUES");

            foreach (var streamMapIt in streamMaps.AsSmartEnumerable())
            {
                streaming_service_anime_map streamMap = streamMapIt.Value;
                if (!streamMapIt.IsFirst)
                {
                    sql.AppendLine(",");
                }
                sql.AppendFormat("({0}, {1}, {2}, {3})",
                    streamMap.mal_anime_id.ToString(CultureInfo.InvariantCulture),
                    streamMap.streaming_service_id.ToString(CultureInfo.InvariantCulture),
                    PgHelpers.QuotePgString(streamMap.streaming_url),
                    streamMap.requires_subscription.ToString()
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