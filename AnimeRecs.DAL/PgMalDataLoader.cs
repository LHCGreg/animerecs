using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using MalApi;
using Npgsql;
using Dapper;

namespace AnimeRecs.DAL
{
    public class PgMalDataLoader : IMalTrainingDataLoader, IDisposable
    {
        private string m_connectionString;

        public PgMalDataLoader(string connectionString)
        {
            m_connectionString = connectionString;
        }

        private class mal_list_entry_slim
        {
            public int mal_user_id { get; set; }
            public int mal_anime_id { get; set; }
            public short? rating { get; set; }
            public short mal_list_entry_status_id { get; set; }
            public short num_episodes_watched { get; set; }
        }

        public MalTrainingData LoadMalTrainingData()
        {
            // Load all anime, then all users, then all entries

            Dictionary<int, Dictionary<int, MalListEntry>> userAnimeLists = new Dictionary<int, Dictionary<int, MalListEntry>>();

            Dictionary<int, mal_user> dbUsers = new Dictionary<int, mal_user>();

            Logging.Log.Debug("Connecting to PostgreSQL.");
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                conn.Open();
                Logging.Log.Debug("Connected to PostgreSQL.");

                Logging.Log.Debug("Slurping anime from the database.");
                IEnumerable<mal_anime> dbAnimeSlurp = mal_anime.GetAll(conn, transaction: null);
                Logging.Log.Debug("Processing anime from the database.");
                Dictionary<int, MalAnime> animes = new Dictionary<int, MalAnime>(dbAnimeSlurp.Count());
                foreach (mal_anime dbAnime in dbAnimeSlurp)
                {
                    MalAnime anime = new MalAnime(
                        malAnimeId: dbAnime.mal_anime_id,
                        type: (MalAnimeType)dbAnime.mal_anime_type_id,
                        title: dbAnime.title
                    );
                    animes[dbAnime.mal_anime_id] = anime;
                }
                Logging.Log.DebugFormat("Done processing {0} anime from the database.", animes.Count);

                Logging.Log.Debug("Slurping users from the database.");
                IEnumerable<mal_user> dbUserSlurp = mal_user.GetAll(conn, transaction: null);
                Logging.Log.Debug("Processing users from the database.");
                foreach (mal_user dbUser in dbUserSlurp)
                {
                    dbUsers[dbUser.mal_user_id] = dbUser;
                }
                Logging.Log.DebugFormat("Done processing {0} users from the database.", dbUsers.Count);

                string allEntriesSlimSql = @"
SELECT mal_user_id, mal_anime_id, rating, mal_list_entry_status_id, num_episodes_watched
FROM mal_list_entry
";

                Logging.Log.Debug("Slurping list entries from the database.");
                // Do not buffer list entries
                IEnumerable<mal_list_entry_slim> dbEntrySlurp = conn.Query<mal_list_entry_slim>(allEntriesSlimSql, buffered: false, commandTimeout: 60);
                Logging.Log.Debug("Processing list entries from the database.");
                long entryCount = 0;

                Dictionary<int, List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>> entriesByUser
                    = new Dictionary<int, List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>>();

                foreach (mal_list_entry_slim dbEntry in dbEntrySlurp)
                {
                    entryCount++;
                    mal_user dbUser;
                    if(!dbUsers.TryGetValue(dbEntry.mal_user_id, out dbUser) || !animes.ContainsKey(dbEntry.mal_anime_id))
                    {
                        // Entry for an anime or user that wasn't in the database...there must have been an update going on between the time we got users, anime, and list entries
                        continue;
                    }
                    List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId> animeList;
                    if (!entriesByUser.TryGetValue(dbEntry.mal_user_id, out animeList))
                    {
                        animeList = new List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>();
                        entriesByUser[dbEntry.mal_user_id] = animeList;
                    }

                    animeList.Add(new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(
                        animeId: dbEntry.mal_anime_id,
                        entry: new MalListEntry(
                            rating: (byte?)dbEntry.rating,
                            status: (CompletionStatus)dbEntry.mal_list_entry_status_id,
                            numEpisodesWatched: dbEntry.num_episodes_watched
                        )
                    ));
                }

                Dictionary<int, MalUserListEntries> users = new Dictionary<int, MalUserListEntries>(dbUserSlurp.Count());
                foreach (int userId in entriesByUser.Keys)
                {
                    List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId> animeList = entriesByUser[userId];
                    animeList.Capacity = animeList.Count;
                    ReadOnlyMalListEntryDictionary listEntries = new ReadOnlyMalListEntryDictionary(animeList);
                    users[userId] = new MalUserListEntries(listEntries, animes, dbUsers[userId].mal_name);
                }

                Logging.Log.DebugFormat("Done processing {0} list entries.", entryCount);

                return new MalTrainingData(users, animes);
            }
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