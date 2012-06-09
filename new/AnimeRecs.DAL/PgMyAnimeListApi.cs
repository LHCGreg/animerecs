using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Extensions;
using AnimeRecs.MalApi;

namespace AnimeRecs.DAL
{
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
            public int mal_user_id { get; set; }
            public string mal_name { get; set; }
            public int mal_anime_id { get; set; }
            public string title { get; set; }
            public int mal_anime_type_id { get; set; }
            public decimal? rating { get; set; }
            public int mal_list_entry_status_id { get; set; }
            public int num_episodes_watched { get; set; }
        }

        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            using (PetaPoco.Database db = new PetaPoco.Database(m_connectionString, Npgsql.NpgsqlFactory.Instance))
            {
                // ILIKE does a table scan. :( The ideal way of doing a case-insensitive search would be to use the citext
                // data type, but that's an add-on and not part of a standard Postgres install.
                // This class is only intended to be used for development anyway.

                string sql = @"
SELECT mal_user.mal_user_id, mal_user.mal_name,
mal_anime.mal_anime_id, mal_anime.title, mal_anime.mal_anime_type_id,
mal_list_entry.rating, mal_list_entry.mal_list_entry_status_id, mal_list_entry.num_episodes_watched

FROM mal_user
JOIN mal_list_entry ON mal_user.mal_user_id = mal_list_entry.mal_user_id
JOIN mal_anime ON mal_list_entry.mal_anime_id = mal_anime.mal_anime_id

WHERE mal_user.mal_name ILIKE @0";

                MalUserLookupResults results = new MalUserLookupResults()
                {
                    AnimeList = new List<MyAnimeListEntry>()
                };

                foreach (UserListEntry dbEntry in db.Query<UserListEntry>(sql, user))
                {
                    results.UserId = dbEntry.mal_user_id;
                    results.CanonicalUserName = dbEntry.mal_name;
                    MyAnimeListEntry entry = new MyAnimeListEntry()
                    {
                        AnimeInfo = new MalAnimeInfoFromUserLookup()
                        {
                            AnimeId = dbEntry.mal_anime_id,
                            Title = dbEntry.title,
                            Type = (MalAnimeType)dbEntry.mal_anime_type_id
                        },
                        NumEpisodesWatched = dbEntry.num_episodes_watched,
                        Score = dbEntry.rating,
                        Status = (CompletionStatus)dbEntry.mal_list_entry_status_id
                    };
                }

                if(results.CanonicalUserName == null)
                {
                    throw new MalUserNotFoundException(string.Format("User \"{0}\" not found in the database.", user));
                }
                else
                {
                    return results;
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