using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecEngine.MAL;
using MalApi;
using Npgsql;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.Utils;

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

        public async Task<MalTrainingData> LoadMalTrainingDataAsync(CancellationToken cancellationToken)
        {
            // Load all anime, users, and entries in parallel, then combine them into training data.
            try
            {
                Dictionary<int, MalAnime> animes;
                Dictionary<int, mal_user> dbUsers;
                IList<mal_list_entry_slim> dbEntrySlurp;

                using (CancellationTokenSource faultCanceler = new CancellationTokenSource())
                using (CancellationTokenSource faultOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, faultCanceler.Token))
                {
                    Task<Dictionary<int, MalAnime>> animeTask = AsyncUtils.EnsureExceptionsWrapped(
                        () => SlurpAnimeAsync(faultOrUserCancel.Token));
                    CancellableTask cancellableAnimeTask = new CancellableTask(animeTask, faultCanceler);

                    Task<Dictionary<int, mal_user>> userTask = AsyncUtils.EnsureExceptionsWrapped(
                        () => SlurpUsersAsync(faultOrUserCancel.Token));
                    CancellableTask cancellableUserTask = new CancellableTask(userTask, faultCanceler);

                    Task<IList<mal_list_entry_slim>> entryTask = AsyncUtils.EnsureExceptionsWrapped(
                        () => SlurpEntriesAsync(faultOrUserCancel.Token));
                    CancellableTask cancellableEntryTask = new CancellableTask(entryTask, faultCanceler);

                    await AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(cancellableEntryTask, cancellableAnimeTask, cancellableUserTask).ConfigureAwait(false);

                    animes = animeTask.Result;
                    dbUsers = userTask.Result;
                    dbEntrySlurp = entryTask.Result;
                }

                Logging.Log.Debug("Processing list entries from the database.");
                long entryCount = 0;

                Dictionary<int, List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>> entriesByUser =
                    new Dictionary<int, List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>>();

                foreach (mal_list_entry_slim dbEntry in dbEntrySlurp)
                {
                    entryCount++;
                    mal_user dbUser;
                    if (!dbUsers.TryGetValue(dbEntry.mal_user_id, out dbUser) || !animes.ContainsKey(dbEntry.mal_anime_id))
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

                Dictionary<int, MalUserListEntries> users = new Dictionary<int, MalUserListEntries>(dbUsers.Count);
                foreach (int userId in entriesByUser.Keys)
                {
                    List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId> animeList = entriesByUser[userId];
                    animeList.Capacity = animeList.Count;
                    ReadOnlyMalListEntryDictionary listEntries = new ReadOnlyMalListEntryDictionary(animeList);
                    users[userId] = new MalUserListEntries(listEntries, animes, dbUsers[userId].mal_name, okToRecommendPredicate: null);
                }

                Logging.Log.DebugFormat("Done processing {0} list entries.", entryCount);

                return new MalTrainingData(users, animes);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error loading MAL training data: {0}", ex.Message), ex);
            }
        }

        private async Task<Dictionary<int, MalAnime>> SlurpAnimeAsync(CancellationToken cancellationToken)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                Logging.Log.Debug("Connecting to PostgreSQL.");
                await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Connected to PostgreSQL.");

                Logging.Log.Debug("Slurping anime from the database.");
                IList<mal_anime> dbAnimeSlurp = await mal_anime.GetAllAsync(conn, transaction: null, cancellationToken: cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Processing anime from the database.");
                Dictionary<int, MalAnime> animes = new Dictionary<int, MalAnime>(dbAnimeSlurp.Count);
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
                return animes;
            }
        }

        private async Task<Dictionary<int, mal_user>> SlurpUsersAsync(CancellationToken cancellationToken)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                Logging.Log.Debug("Connecting to PostgreSQL.");
                await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Connected to PostgreSQL.");

                Logging.Log.Debug("Slurping users from the database.");
                IList<mal_user> dbUserSlurp = await mal_user.GetAllAsync(conn, transaction: null, cancellationToken: cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Processing users from the database.");
                Dictionary<int, mal_user> dbUsers = new Dictionary<int, mal_user>(dbUserSlurp.Count);
                foreach (mal_user dbUser in dbUserSlurp)
                {
                    dbUsers[dbUser.mal_user_id] = dbUser;
                }
                Logging.Log.DebugFormat("Done processing {0} users from the database.", dbUsers.Count);
                return dbUsers;
            }
        }

        private async Task<IList<mal_list_entry_slim>> SlurpEntriesAsync(CancellationToken cancellationToken)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                Logging.Log.Debug("Connecting to PostgreSQL.");
                await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Connected to PostgreSQL.");

                string sql = @"
SELECT mal_user_id, mal_anime_id, rating, mal_list_entry_status_id, num_episodes_watched
FROM mal_list_entry";
                TimeSpan timeout = TimeSpan.FromSeconds(60); // TODO: make this configurable
                Logging.Log.Debug("Slurping list entries from the database.");

                try
                {
                    return await conn.QueryAsyncWithCancellation<mal_list_entry_slim>(sql, timeout, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    throw new Exception(string.Format("Error loading all MAL list entries from database: {0}", ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// Returns a dictionary where the key is the MAL anime id of an anime and the value is a list of MAL anime ids of prerequisites
        /// for that anime. If a key is not in the dictionary, that anime has no known prerequisites.
        /// </summary>
        /// <returns></returns>
        public async Task<IDictionary<int, IList<int>>> LoadPrerequisitesAsync(CancellationToken cancellationToken)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(m_connectionString))
            {
                Logging.Log.Debug("Connecting to PostgreSQL.");
                await conn.OpenAsync(cancellationToken);
                Logging.Log.Debug("Connected to PostgreSQL.");

                Dictionary<int, IList<int>> prereqs = new Dictionary<int, IList<int>>();
                Logging.Log.Debug("Slurping prerequisites from the database.");
                foreach (mal_anime_prerequisite prereq in await mal_anime_prerequisite.GetAllAsync(conn, transaction: null, cancellationToken: cancellationToken).ConfigureAwait(false))
                {
                    if (!prereqs.ContainsKey(prereq.mal_anime_id))
                    {
                        prereqs[prereq.mal_anime_id] = new List<int>(1);
                    }
                    prereqs[prereq.mal_anime_id].Add(prereq.prerequisite_mal_anime_id);
                }
                Logging.Log.Debug("Done slurping prerequisites.");
                return prereqs;
            }

        }

        public void Dispose()
        {
            ;
        }
    }
}
