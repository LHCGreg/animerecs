using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Extensions;
using MiscUtil.Collections.Extensions;
using MalApi;
using Npgsql;
using Dapper;

namespace AnimeRecs.DAL
{
    /// <summary>
    /// Acts as a MAL API that gets users' anime lists from a local database instead of MAL.
    /// </summary>
    public class PgMyAnimeListApi : IMyAnimeListApi
    {
        private string m_connectionString;

        public PgMyAnimeListApi(string connectionString)
        {
            connectionString.ThrowIfNull("connectionString");
            m_connectionString = connectionString;
        }

        private class UserListEntry
        {
            public int mal_anime_id { get; set; }
            public string title { get; set; }
            public int mal_anime_type_id { get; set; }
            public int num_episodes { get; set; }
            public int mal_anime_status_id { get; set; }
            public int? start_year { get; set; }
            public int? start_month { get; set; }
            public int? start_day { get; set; }
            public int? end_year { get; set; }
            public int? end_month { get; set; }
            public int? end_day { get; set; }
            public string image_url { get; set; }
            public IEnumerable<string> synonyms { get; set; } // ignore any nulls in this

            public decimal? rating { get; set; }
            public int mal_list_entry_status_id { get; set; }
            public int num_episodes_watched { get; set; }
            public int? started_watching_year { get; set; }
            public int? started_watching_month { get; set; }
            public int? started_watching_day { get; set; }
            public int? finished_watching_year { get; set; }
            public int? finished_watching_month { get; set; }
            public int? finished_watching_day { get; set; }
            public DateTime last_mal_update { get; set; }
            public IEnumerable<string> tags { get; set; } // ignore any nulls in this
        }

        private class User
        {
            public string mal_name { get; set; }
            public int mal_user_id { get; set; }
        }

        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                conn.Open();
                // ILIKE does a table scan. :( The ideal way of doing a case-insensitive search would be to use the citext
                // data type, but that's an add-on and not part of a standard Postgres install.
                // This class is only intended to be used for development anyway.

                string sql = @"
SELECT mal_user.mal_name, mal_user.mal_user_id FROM mal_user WHERE mal_name ILIKE :UserName ORDER BY mal_user_id LIMIT 1;

SELECT

mal_anime.mal_anime_id, mal_anime.title, mal_anime.mal_anime_type_id, mal_anime.num_episodes, mal_anime.mal_anime_status_id,
mal_anime.start_year, mal_anime.start_month, mal_anime.start_day, mal_anime.end_year, mal_anime.end_month, mal_anime.end_day,
mal_anime.image_url,

mal_list_entry.rating, mal_list_entry.mal_list_entry_status_id, mal_list_entry.num_episodes_watched,
mal_list_entry.started_watching_year, mal_list_entry.started_watching_month, mal_list_entry.started_watching_day,
mal_list_entry.finished_watching_year, finished_watching_month, finished_watching_day, mal_list_entry.last_mal_update,

mal_list.synonyms, mal_list.tags

FROM
(
SELECT mal_user.mal_user_id AS mal_user_id, mal_anime.mal_anime_id AS mal_anime_id, array_agg(DISTINCT mal_anime_synonym.synonym) AS synonyms, array_agg(DISTINCT mal_list_entry_tag.tag) AS tags

FROM mal_user
JOIN mal_list_entry ON mal_user.mal_user_id = mal_list_entry.mal_user_id
JOIN mal_anime ON mal_list_entry.mal_anime_id = mal_anime.mal_anime_id
LEFT OUTER JOIN mal_anime_synonym ON mal_anime.mal_anime_id = mal_anime_synonym.mal_anime_id
LEFT OUTER JOIN mal_list_entry_tag ON mal_list_entry.mal_anime_id = mal_list_entry_tag.mal_anime_id AND mal_list_entry.mal_user_id = mal_list_entry_tag.mal_user_id

WHERE mal_user.mal_user_id = (SELECT mal_user_id FROM mal_user WHERE mal_name ILIKE :UserName ORDER BY mal_user_id LIMIT 1)
GROUP BY mal_user.mal_user_id, mal_anime.mal_anime_id
) AS mal_list
JOIN mal_user ON mal_list.mal_user_id = mal_user.mal_user_id
JOIN mal_anime ON mal_list.mal_anime_id = mal_anime.mal_anime_id
JOIN mal_list_entry ON mal_list.mal_user_id = mal_list_entry.mal_user_id AND mal_list.mal_anime_id = mal_list_entry.mal_anime_id
";
                int userId;
                string canonicalUserName = null;
                List<MyAnimeListEntry> entries = new List<MyAnimeListEntry>();
                using (SqlMapper.GridReader results = conn.QueryMultiple(sql, new { UserName = user }))
                {
                    User u = results.Read<User>().FirstOrDefault();
                    if (u == null)
                    {
                        throw new MalUserNotFoundException(string.Format("No MAL list exists for {0}.", user));
                    }
                    userId = u.mal_user_id;
                    canonicalUserName = u.mal_name;

                    foreach (UserListEntry dbEntry in results.Read<UserListEntry>())
                    {
                        MalAnimeInfoFromUserLookup animeInfo = new MalAnimeInfoFromUserLookup(
                            animeId: dbEntry.mal_anime_id,
                            title: dbEntry.title,
                            type: (MalAnimeType)dbEntry.mal_anime_type_id,
                            synonyms: dbEntry.synonyms.Where(syn => syn != null).ToList(),
                            status: (MalSeriesStatus)dbEntry.mal_anime_status_id,
                            numEpisodes: dbEntry.num_episodes,
                            startDate: new UncertainDate(year: dbEntry.start_year, month: dbEntry.start_month, day: dbEntry.start_day),
                            endDate: new UncertainDate(year: dbEntry.end_year, month: dbEntry.end_month, day: dbEntry.end_day),
                            imageUrl: dbEntry.image_url
                        );

                        MyAnimeListEntry entry = new MyAnimeListEntry(
                            score: dbEntry.rating,
                            status: (CompletionStatus)dbEntry.mal_list_entry_status_id,
                            numEpisodesWatched: dbEntry.num_episodes_watched,
                            myStartDate: new UncertainDate(year: dbEntry.started_watching_year, month: dbEntry.started_watching_month, day: dbEntry.started_watching_day),
                            myFinishDate: new UncertainDate(year: dbEntry.finished_watching_year, month: dbEntry.finished_watching_month, day: dbEntry.finished_watching_day),
                            myLastUpdate: dbEntry.last_mal_update,
                            animeInfo: animeInfo,
                            tags: dbEntry.tags.Where(tag => tag != null).ToList()
                        );

                        entries.Add(entry);
                    }

                    return new MalUserLookupResults(userId: userId, canonicalUserName: canonicalUserName, animeList: entries);
                }
            }
        }

        /// <exception cref="System.NotImplementedException">Always thrown.</exception>
        public RecentUsersResults GetRecentOnlineUsers()
        {
            throw new NotImplementedException("PostgreSQL MAL API does not support getting recent online users.");
        }

        public void Dispose()
        {
            ;
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