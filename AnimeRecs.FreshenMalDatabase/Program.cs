using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using AnimeRecs.DAL;
using AnimeRecs.MalApi;

namespace AnimeRecs.FreshenMalDatabase
{
    class Program
    {
        static Config config;

        static void Main(string[] args)
        {
            Logging.SetUpLogging();

            try
            {
                config = new Config();

                using (IMyAnimeListApi basicApi = new MyAnimeListApi() { TimeoutInMs = config.MalTimeoutInMs, UserAgent = config.MalApiUserAgentString })
                using (IMyAnimeListApi cachingApi = new CachingMyAnimeListApi(basicApi, expiration: null))
                using (IMyAnimeListApi rateLimitingApi = new RateLimitingMyAnimeListApi(cachingApi, TimeSpan.FromMilliseconds(config.DelayBetweenRequestsInMs)))
                using (IMyAnimeListApi malApi = new RetryOnFailureMyAnimeListApi(rateLimitingApi, config.NumMalRequestFailuresBeforeGivingUp, config.DelayAfterMalRequestFailureInMs))
                using (PetaPoco.Database db = new PetaPoco.Database(config.PostgresConnectionString, Npgsql.NpgsqlFactory.Instance))
                {
                    int usersAddedSoFar = 0;
                    while (usersAddedSoFar < config.UsersPerRun)
                    {
                        RecentUsersResults recentMalUsers = malApi.GetRecentOnlineUsers();

                        foreach (string user in recentMalUsers.RecentUsers)
                        {
                            if (!UserIsInDatabase(user, db))
                            {
                                MalUserLookupResults userLookup = malApi.GetAnimeListForUser(user);
                                if (UserMeetsCriteria(userLookup, db))
                                {
                                    InsertUserAndRatingsInDatabase(db, userLookup);
                                    usersAddedSoFar++;

                                    if (usersAddedSoFar == config.UsersPerRun)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    TrimDatabaseToMaxUsers(db, config.MaxUsersInDatabase);
                }
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
            }
        }

        /// <summary>
        /// Not a definitive check. If it returns false, you should still check if the id is in the DB in case the user
        /// changed their username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        static bool UserIsInDatabase(string username, PetaPoco.Database db)
        {
            long count = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_user WHERE mal_name = @0", username);
            return count > 0;
        }

        static bool UserMeetsCriteria(MalUserLookupResults userLookup, PetaPoco.Database db)
        {
            // completed, rated >= X, and user is not in DB
            int completedRated = userLookup.AnimeList.Count(anime => anime.Score.HasValue && anime.Status == CompletionStatus.Completed);
            if (completedRated < config.MinimumAnimesCompletedAndRated)
            {
                return false;
            }

            long count = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_user WHERE mal_user_id = @0", userLookup.UserId);
            return count == 0;
        }

        static void InsertUserAndRatingsInDatabase(PetaPoco.Database db, MalUserLookupResults userLookup)
        {
            mal_user user = new mal_user()
            {
                mal_user_id = userLookup.UserId,
                mal_name = userLookup.CanonicalUserName,
                time_added = DateTime.UtcNow
            };

            db.Insert(tableName: "mal_user", primaryKeyName: "mal_user_id", autoIncrement: false, poco: user);

            foreach (MyAnimeListEntry anime in userLookup.AnimeList)
            {
                mal_anime animeRow = new mal_anime()
                {
                    mal_anime_id = anime.AnimeInfo.AnimeId,
                    title = anime.AnimeInfo.Title,
                    mal_anime_type_id = (int)anime.AnimeInfo.Type,
                    last_updated = DateTime.UtcNow
                };

                long oneIfAnimeIsInDb = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_anime WHERE mal_anime_id = @0", anime.AnimeInfo.AnimeId);
                if (oneIfAnimeIsInDb < 1)
                {
                    db.Insert(tableName: "mal_anime", primaryKeyName: "mal_anime_id", autoIncrement: false, poco: animeRow);
                }
                else
                {
                    db.Update(tableName: "mal_anime", primaryKeyName: "mal_anime_id", poco: animeRow);
                }

                mal_list_entry rating = new mal_list_entry()
                {
                    mal_anime_id = anime.AnimeInfo.AnimeId,
                    mal_user_id = userLookup.UserId,
                    num_episodes_watched = anime.NumEpisodesWatched,
                    rating = anime.Score,
                    mal_list_entry_status_id = (int)anime.Status
                };
                db.Insert(tableName: "mal_list_entry", primaryKeyName: "mal_list_entry_id", autoIncrement: true, poco: rating);
            }
        }

        static void TrimDatabaseToMaxUsers(PetaPoco.Database db, long maxUsersInDatabase)
        {
            row_count userCount = db.Single<row_count>(@"SELECT * FROM row_count WHERE table_name = 'mal_user'");
            if (userCount.num_rows > maxUsersInDatabase)
            {
                long numUsersToDelete = userCount.num_rows - maxUsersInDatabase;
                string deleteSql = @"DELETE FROM mal_user WHERE mal_user_id IN
(SELECT mal_user_id FROM mal_user
ORDER BY time_added
LIMIT @0)";

                db.Execute(deleteSql, numUsersToDelete);
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.FreshenMalDatabase.
//
// AnimeRecs.FreshenMalDatabase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.FreshenMalDatabase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.FreshenMalDatabase.  If not, see <http://www.gnu.org/licenses/>.