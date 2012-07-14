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
            Logging.Log.DebugFormat("Checking if {0} is in the database.");
            long count = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_user WHERE mal_name = @0", username);
            bool isInDb = count > 0;
            Logging.Log.DebugFormat("{0} in database = {1}", username, isInDb);
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

            Logging.Log.DebugFormat("Really checking if {0} is in the database by user id.", userLookup.CanonicalUserName);
            long count = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_user WHERE mal_user_id = @0", userLookup.UserId);
            Logging.Log.DebugFormat("{0} in really in database = {1}", userLookup.CanonicalUserName, count > 0);
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

            Logging.Log.DebugFormat("Inserting {0} into DB.", userLookup.CanonicalUserName);
            db.Insert(tableName: "mal_user", primaryKeyName: "mal_user_id", autoIncrement: false, poco: user);
            Logging.Log.DebugFormat("Inserted {0} into DB.", userLookup.CanonicalUserName);

            Logging.Log.DebugFormat("Inserting anime and list entries for {0}.", userLookup.CanonicalUserName);
            foreach (MyAnimeListEntry anime in userLookup.AnimeList)
            {
                mal_anime animeRow = new mal_anime()
                {
                    mal_anime_id = anime.AnimeInfo.AnimeId,
                    title = anime.AnimeInfo.Title,
                    mal_anime_type_id = (int)anime.AnimeInfo.Type,
                    last_updated = DateTime.UtcNow
                };

                Logging.Log.TraceFormat("Checking if anime \"{0}\" is in the database.", anime.AnimeInfo.Title);
                long oneIfAnimeIsInDb = db.ExecuteScalar<long>(@"SELECT Count(*) FROM mal_anime WHERE mal_anime_id = @0", anime.AnimeInfo.AnimeId);
                if (oneIfAnimeIsInDb < 1)
                {
                    Logging.Log.Trace("Not in database. Inserting it.");
                    db.Insert(tableName: "mal_anime", primaryKeyName: "mal_anime_id", autoIncrement: false, poco: animeRow);
                    Logging.Log.TraceFormat("Inserted anime \"{0}\" in database.", anime.AnimeInfo.Title);
                }
                else
                {
                    Logging.Log.TraceFormat("Already in database. Updating it.");
                    db.Update(tableName: "mal_anime", primaryKeyName: "mal_anime_id", poco: animeRow);
                    Logging.Log.TraceFormat("Updated anime \"{0}\".", anime.AnimeInfo.Title);
                }

                mal_list_entry rating = new mal_list_entry()
                {
                    mal_anime_id = anime.AnimeInfo.AnimeId,
                    mal_user_id = userLookup.UserId,
                    num_episodes_watched = anime.NumEpisodesWatched,
                    rating = anime.Score,
                    mal_list_entry_status_id = (int)anime.Status
                };

                Logging.Log.TraceFormat("Inserting list entry for user \"{0}\", anime \"{1}\"", userLookup.CanonicalUserName, anime.AnimeInfo.Title);
                db.Insert(tableName: "mal_list_entry", primaryKeyName: "mal_list_entry_id", autoIncrement: true, poco: rating);
                Logging.Log.TraceFormat("Inserted list entry for user \"{0}\", anime \"{1}\"", userLookup.CanonicalUserName, anime.AnimeInfo.Title);
            }

            Logging.Log.DebugFormat("Done inserting anime and list entries for {0}.", userLookup.CanonicalUserName);
        }

        static void TrimDatabaseToMaxUsers(PetaPoco.Database db, long maxUsersInDatabase)
        {
            Logging.Log.DebugFormat("Trimming database to {0} users.", maxUsersInDatabase);
            row_count userCount = db.Single<row_count>(@"SELECT * FROM row_count WHERE table_name = 'mal_user'");
            Logging.Log.DebugFormat("{0} users are in the database.", userCount.num_rows);

            if (userCount.num_rows > maxUsersInDatabase)
            {
                long numUsersToDelete = userCount.num_rows - maxUsersInDatabase;
                Logging.Log.DebugFormat("Deleting {0} users.", numUsersToDelete);

                string deleteSql = @"DELETE FROM mal_user WHERE mal_user_id IN
(SELECT mal_user_id FROM mal_user
ORDER BY time_added
LIMIT @0)";

                db.Execute(deleteSql, numUsersToDelete);

                Logging.Log.DebugFormat("Deleted {0} users.", numUsersToDelete);
            }
            else
            {
                Logging.Log.Debug("Don't need to delete any users.");
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