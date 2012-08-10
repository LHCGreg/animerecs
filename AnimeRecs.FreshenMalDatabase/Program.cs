using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Collections.Extensions;
using AnimeRecs.DAL;
using AnimeRecs.MalApi;
using Dapper;
using Npgsql;
using System.Globalization;

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
                using (NpgsqlConnection conn = new NpgsqlConnection(config.PostgresConnectionString))
                {
                    conn.Open();
                    int usersAddedSoFar = 0;
                    while (usersAddedSoFar < config.UsersPerRun)
                    {
                        RecentUsersResults recentMalUsers = malApi.GetRecentOnlineUsers();

                        foreach (string user in recentMalUsers.RecentUsers)
                        {
                            using (var transaction = conn.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                            {
                                if (!UserIsInDatabase(user, conn, transaction))
                                {
                                    MalUserLookupResults userLookup = malApi.GetAnimeListForUser(user);
                                    if (UserMeetsCriteria(userLookup, conn, transaction))
                                    {
                                        InsertUserAndRatingsInDatabase(userLookup, conn, transaction);
                                        usersAddedSoFar++;
                                        Logging.Log.Debug("Committing transaction.");
                                        transaction.Commit();
                                        Logging.Log.Debug("Transaction committed.");

                                        if (usersAddedSoFar == config.UsersPerRun)
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Logging.Log.InfoFormat("{0} does not meet criteria for inclusion, skipping", user);
                                    }
                                }
                                else
                                {
                                    Logging.Log.InfoFormat("{0} is already in the database, skipping.", user);
                                }
                            }
                        }
                    }

                    using (var transaction = conn.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                    {
                        TrimDatabaseToMaxUsers(config.MaxUsersInDatabase, conn, transaction);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// Not a definitive check. If it returns false, you should still check if the id is in the DB in case the user
        /// changed their username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        static bool UserIsInDatabase(string username, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            Logging.Log.DebugFormat("Checking if {0} is in the database.", username);
            long count = conn.Query<long>(@"SELECT Count(*) FROM mal_user WHERE mal_name = :Username", new { Username = username }, transaction).First();
            bool isInDb = count > 0;
            Logging.Log.DebugFormat("{0} in database = {1}", username, isInDb);
            return isInDb;
        }

        static bool UserMeetsCriteria(MalUserLookupResults userLookup, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            // completed, rated >= X, and user is not in DB
            int completedRated = userLookup.AnimeList.Count(anime => anime.Score.HasValue && anime.Status == CompletionStatus.Completed);
            if (completedRated < config.MinimumAnimesCompletedAndRated)
            {
                return false;
            }

            Logging.Log.DebugFormat("Really checking if {0} is in the database by user id.", userLookup.CanonicalUserName);
            long count = conn.Query<long>(@"SELECT Count(*) FROM mal_user WHERE mal_user_id = :UserId", new { UserId = userLookup.UserId }, transaction).First();
            Logging.Log.DebugFormat("{0} in really in database = {1}", userLookup.CanonicalUserName, count > 0);
            return count == 0;
        }

        // Only insert/update an anime once per run to save on trips to the DB
        static Dictionary<int, mal_anime> AnimesUpserted = new Dictionary<int, mal_anime>();

        static void InsertUserAndRatingsInDatabase(MalUserLookupResults userLookup, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            Logging.Log.InfoFormat("Inserting anime and list entries for {0} ({1} entries).", userLookup.CanonicalUserName, userLookup.AnimeList.Count);

            List<mal_anime> animesToUpsert = new List<mal_anime>();
            Dictionary<int, List<mal_anime_synonym>> synonymsToUpsert = new Dictionary<int, List<mal_anime_synonym>>();
            List<mal_list_entry> entriesToInsert = new List<mal_list_entry>();
            List<mal_list_entry_tag> tagsToInsert = new List<mal_list_entry_tag>();

            // Buffer animes, anime synonyms, list entries, and tags.
            // For animes not upserted this session, upsert animes all at once, clear synonyms, insert synonyms
            // insert user
            // insert list entries all at once
            // insert tags all at once

            foreach (MyAnimeListEntry anime in userLookup.AnimeList)
            {
                if (!AnimesUpserted.ContainsKey(anime.AnimeInfo.AnimeId))
                {
                    mal_anime animeRow = new mal_anime(
                        _mal_anime_id: anime.AnimeInfo.AnimeId,
                        _title: anime.AnimeInfo.Title,
                        _mal_anime_type_id: (int)anime.AnimeInfo.Type,
                        _num_episodes: anime.AnimeInfo.NumEpisodes,
                        _mal_anime_status_id: (int)anime.AnimeInfo.Status,
                        _start_year: anime.AnimeInfo.StartDate.Year,
                        _start_month: anime.AnimeInfo.StartDate.Month,
                        _start_day: anime.AnimeInfo.StartDate.Day,
                        _end_year: anime.AnimeInfo.EndDate.Year,
                        _end_month: anime.AnimeInfo.EndDate.Month,
                        _end_day: anime.AnimeInfo.EndDate.Day,
                        _image_url: anime.AnimeInfo.ImageUrl,
                        _last_updated: DateTime.UtcNow
                    );

                    animesToUpsert.Add(animeRow);

                    List<mal_anime_synonym> synonymRowsForThisAnime = new List<mal_anime_synonym>();
                    foreach (string synonym in anime.AnimeInfo.Synonyms)
                    {
                        mal_anime_synonym synonymRow = new mal_anime_synonym(
                            _mal_anime_id: anime.AnimeInfo.AnimeId,
                            _synonym: synonym
                        );
                        synonymRowsForThisAnime.Add(synonymRow);
                    }

                    synonymsToUpsert[anime.AnimeInfo.AnimeId] = synonymRowsForThisAnime;
                }

                mal_list_entry dbListEntry = new mal_list_entry(
                    _mal_user_id: userLookup.UserId,
                    _mal_anime_id: anime.AnimeInfo.AnimeId,
                    _rating: anime.Score,
                    _mal_list_entry_status_id: (int)anime.Status,
                    _num_episodes_watched: anime.NumEpisodesWatched,
                    _started_watching_year: anime.MyStartDate.Year,
                    _started_watching_month: anime.MyStartDate.Month,
                    _started_watching_day: anime.MyStartDate.Day,
                    _finished_watching_year: anime.MyFinishDate.Year,
                    _finished_watching_month: anime.MyFinishDate.Month,
                    _finished_watching_day: anime.MyFinishDate.Day,
                    _last_mal_update: anime.MyLastUpdate
                );

                entriesToInsert.Add(dbListEntry);

                foreach (string tag in anime.Tags)
                {
                    mal_list_entry_tag dbTag = new mal_list_entry_tag(
                        _mal_user_id: userLookup.UserId,
                        _mal_anime_id: anime.AnimeInfo.AnimeId,
                        _tag: tag
                    );
                    tagsToInsert.Add(dbTag);
                }
            }

            // For animes not upserted this session, upsert animes, clear synonyms all at once, insert synonyms all at once
            Logging.Log.DebugFormat("Upserting {0} animes.", animesToUpsert.Count);
            foreach (mal_anime animeToUpsert in animesToUpsert)
            {
                Logging.Log.TraceFormat("Checking if anime \"{0}\" is in the database.", animeToUpsert.title);
                long oneIfAnimeIsInDb = conn.Query<long>("SELECT Count(*) FROM mal_anime WHERE mal_anime_id = :AnimeId",
                    new { AnimeId = animeToUpsert.mal_anime_id }, transaction).First();
                if (oneIfAnimeIsInDb < 1)
                {
                    // Not worth optimizing this by batching inserts because once there are a couple hundred users in the database,
                    // inserts will be relatively few in number.
                    Logging.Log.Trace("Not in database. Inserting it.");
                    animeToUpsert.Insert(conn, transaction);
                    Logging.Log.TraceFormat("Inserted anime \"{0}\" in database.", animeToUpsert.title);
                    AnimesUpserted[animeToUpsert.mal_anime_id] = animeToUpsert;
                }
                else
                {
                    Logging.Log.TraceFormat("Already in database. Updating it.");
                    animeToUpsert.Update(conn, transaction);
                    Logging.Log.TraceFormat("Updated anime \"{0}\".", animeToUpsert.title);
                    AnimesUpserted[animeToUpsert.mal_anime_id] = animeToUpsert;
                }
            }
            Logging.Log.DebugFormat("Upserted {0} animes.", animesToUpsert.Count);

            if (synonymsToUpsert.Count > 0)
            {
                List<mal_anime_synonym> flattenedSynonyms = synonymsToUpsert.Values.SelectMany(synonyms => synonyms).ToList();

                // clear synonyms for all these animes
                Logging.Log.DebugFormat("Clearing {0} synonyms for this batch.", flattenedSynonyms.Count);
                mal_anime_synonym.Delete(synonymsToUpsert.Keys, conn, transaction);
                Logging.Log.DebugFormat("Cleared {0} synonyms for this batch.", flattenedSynonyms.Count);

                // insert synonyms for all these animes
                Logging.Log.DebugFormat("Inserting {0} synonyms for this batch.", flattenedSynonyms.Count);
                mal_anime_synonym.Insert(flattenedSynonyms, conn, transaction);
                Logging.Log.DebugFormat("Inserted {0} synonyms for this batch.", flattenedSynonyms.Count);
            }
            else
            {
                Logging.Log.Debug("No synonyms in this batch.");
            }

            // Insert user
            mal_user user = new mal_user(
                _mal_user_id: userLookup.UserId,
                _mal_name: userLookup.CanonicalUserName,
                _time_added: DateTime.UtcNow
            );

            Logging.Log.DebugFormat("Inserting {0} into DB.", userLookup.CanonicalUserName);
            user.Insert(conn, transaction);
            Logging.Log.DebugFormat("Inserted {0} into DB.", userLookup.CanonicalUserName);

            // insert list entries all at once
            if (entriesToInsert.Count > 0)
            {
                Logging.Log.DebugFormat("Inserting {0} list entries for user \"{1}\".", entriesToInsert.Count, userLookup.CanonicalUserName);
                mal_list_entry.Insert(entriesToInsert, conn, transaction);
                Logging.Log.DebugFormat("Inserted {0} list entries for user \"{1}\".", entriesToInsert.Count, userLookup.CanonicalUserName);
            }

            // insert tags all at once
            if (tagsToInsert.Count > 0)
            {
                Logging.Log.DebugFormat("Inserting {0} tags by user \"{1}\".", tagsToInsert.Count, userLookup.CanonicalUserName);
                mal_list_entry_tag.Insert(tagsToInsert, conn, transaction);
                Logging.Log.DebugFormat("Inserted {0} tags by user \"{1}\".", tagsToInsert.Count, userLookup.CanonicalUserName);
            }

            Logging.Log.InfoFormat("Done inserting anime and list entries for {0}.", userLookup.CanonicalUserName);
        }

        static void TrimDatabaseToMaxUsers(long maxUsersInDatabase, NpgsqlConnection conn, NpgsqlTransaction transaction)
        {
            Logging.Log.InfoFormat("Trimming database to {0} users.", maxUsersInDatabase);
            long numUsers = conn.Query<long>("SELECT num_rows FROM row_count WHERE table_name = 'mal_user' LIMIT 1", transaction: transaction).First();
            Logging.Log.DebugFormat("{0} users are in the database.", numUsers);

            if (numUsers > maxUsersInDatabase)
            {
                long numUsersToDelete = numUsers - maxUsersInDatabase;
                Logging.Log.DebugFormat("Deleting {0} users.", numUsersToDelete);

                string deleteSql = @"DELETE FROM mal_user WHERE mal_user_id IN
(SELECT mal_user_id FROM mal_user
ORDER BY time_added
LIMIT :NumToDelete)";

                int numRowsDeleted = conn.Execute(deleteSql, new { NumToDelete = numUsersToDelete }, transaction);

                Logging.Log.InfoFormat("Deleted {0} users.", numUsersToDelete);
            }
            else
            {
                Logging.Log.Info("Don't need to delete any users.");
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