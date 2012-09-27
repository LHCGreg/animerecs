using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MiscUtil.Extensions;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;
using AnimeRecs.DAL;
using System.Diagnostics;

namespace AnimeRecs.RecService
{
    /// <summary>
    /// Stores the state of the recommendation service, including rec sources. All public methods and properties are thread-safe.
    /// </summary>
    internal class RecServiceState : IDisposable
    {
        // When locking, always lock in the order the members are listed here.

        private bool m_finalized = false;
        private MalTrainingData m_trainingData;
        private IDictionary<int, string> m_usernames; // Always update this when updating m_trainingData. Is null when rec sources are finalized.
        private IDictionary<int, RecEngine.MAL.MalAnime> m_animes; // Always update this when updating m_trainingData. Remains when rec sources are finalized.
        private IDictionary<int, IList<int>> m_prereqs; // Update this when reloading.
        private ReaderWriterLockSlim m_trainingDataLock;

        private Dictionary<string, Func<ITrainableJsonRecSource>> m_recSourceFactories = new Dictionary<string, Func<ITrainableJsonRecSource>>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, ITrainableJsonRecSource> m_recSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
        private ReaderWriterLockSlim m_recSourcesLock;

        private IMalTrainingDataLoaderFactory m_trainingDataLoaderFactory;

        // Thread-safe because it doesn't change after construction
        public IDictionary<string, Type> JsonRecSourceTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingDataLoaderFactory">Must be thread-safe.</param>
        public RecServiceState(IMalTrainingDataLoaderFactory trainingDataLoaderFactory)
        {
            m_trainingDataLoaderFactory = trainingDataLoaderFactory;
            JsonRecSourceTypes = GetJsonRecSourceTypes();
            using (IMalTrainingDataLoader trainingDataLoader = trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                m_trainingData = LoadTrainingDataOnInit(trainingDataLoader);
                m_usernames = GetUsernamesFromTrainingData(m_trainingData);
                m_animes = m_trainingData.Animes;
                m_prereqs = LoadPrereqsOnInit(trainingDataLoader);
            }
            m_trainingDataLock = new ReaderWriterLockSlim();
            m_recSourcesLock = new ReaderWriterLockSlim();
        }

        private IDictionary<string, Type> GetJsonRecSourceTypes()
        {
            Logging.Log.Debug("Searching for types that implement ITrainableJsonRecSource and have at least one [JsonRecSource] attribute.");
            Type[] typesInThisAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Type jsonRecSourceInterface = typeof(ITrainableJsonRecSource);
            Type jsonRecSourceAttribute = typeof(JsonRecSourceAttribute);

            Dictionary<string, Type> jsonRecSourceTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (Type type in typesInThisAssembly)
            {
                if (jsonRecSourceInterface.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    List<string> recSourceTypesDeclaredFor = new List<string>();
                    object[] jsonRecSourceAttributes = type.GetCustomAttributes(jsonRecSourceAttribute, inherit: false);
                    foreach (JsonRecSourceAttribute attribute in jsonRecSourceAttributes.Select(attributeObj => (JsonRecSourceAttribute)attributeObj))
                    {
                        recSourceTypesDeclaredFor.Add(attribute.RecSourceName);
                        jsonRecSourceTypes[attribute.RecSourceName] = type;
                    }

                    Logging.Log.DebugFormat("{0} registered for: {1}", type.Name, string.Join(", ", recSourceTypesDeclaredFor));
                }
            }

            Logging.Log.DebugFormat("Done searching for JSON rec sources.");

            return jsonRecSourceTypes;
        }

        private MalTrainingData LoadTrainingDataOnInit(IMalTrainingDataLoader trainingDataLoader)
        {
            Logging.Log.Info("Loading training data.");
            Stopwatch timer = Stopwatch.StartNew();
            MalTrainingData trainingData = trainingDataLoader.LoadMalTrainingData();
            GC.Collect();
            timer.Stop();

            Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                trainingData.Users.Count, trainingData.Animes.Count,
                trainingData.Users.Keys.Sum(userId => trainingData.Users[userId].Entries.Count),
                timer.Elapsed);
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            return trainingData;
        }

        private IDictionary<int, IList<int>> LoadPrereqsOnInit(IMalTrainingDataLoader trainingDataLoader)
        {
            Logging.Log.Info("Loading prerequisites.");
            Stopwatch timer = Stopwatch.StartNew();
            IDictionary<int, IList<int>> prereqs = trainingDataLoader.LoadPrerequisites();
            timer.Stop();

            int numPrereqs = prereqs.Values.Sum(prereqList => prereqList.Count);
            Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                numPrereqs, prereqs.Count, timer.Elapsed);
            Logging.Log.InfoFormat("Memory use {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            return prereqs;
        }

        public void LoadRecSource(Func<ITrainableJsonRecSource> recSourceFactory, string name, bool replaceExisting)
        {
            ITrainableJsonRecSource recSource = recSourceFactory();
            Logging.Log.InfoFormat("Loading rec source with name \"{0}\", replaceExisting={1}: {2}", name, replaceExisting, recSource);

            // Need to hold read lock on training data while training so that a retrain can't happen while we're training here.
            // Rec sources must be trained with the current m_trainingData, not an old version.
            using (var trainingDataReadLock = m_trainingDataLock.ScopedReadLock())
            using (var recSourcesUpgradeableReadLock = m_recSourcesLock.ScopedUpgradeableReadLock())
            {
                if (m_trainingData == null && !m_finalized)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.NoTrainingData,
                        message: "A reload/retrain in low memory mode failed, leaving the rec service without training data or rec sources. Issue a ReloadTrainingData command to load training data, then load rec sources."));
                }
                else if (m_trainingData == null && m_finalized)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Finalized,
                        message: "Rec sources have been finalized. A non-finalized retrain must be invoked to be able to add rec sources."));
                }

                if (m_recSources.ContainsKey(name) && !replaceExisting)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Unknown,
                        message: string.Format("A recommendation source with the name \"{0}\" already exists.", name)));
                }

                Logging.Log.InfoFormat("Training rec source {0}.", name);
                Stopwatch timer = Stopwatch.StartNew();

                recSource.Train(m_trainingData, m_usernames);

                timer.Stop();
                Logging.Log.InfoFormat("Trained rec source {0}. Took {1}", name, timer.Elapsed);

                using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
                {
                    m_recSources[name] = recSource;
                    m_recSourceFactories[name] = recSourceFactory;
                }
                Logging.Log.InfoFormat("Loaded rec source {0}.", name);

                GC.Collect();
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
            }
        }

        private static IDictionary<int, string> GetUsernamesFromTrainingData(MalTrainingData trainingData)
        {
            Dictionary<int, string> usernames = new Dictionary<int, string>(trainingData.Users.Count);
            foreach (int userId in trainingData.Users.Keys)
            {
                usernames[userId] = trainingData.Users[userId].MalUsername;
            }
            return usernames;
        }

        public void UnloadRecSource(string name)
        {
            name.ThrowIfNull("name");
            Logging.Log.DebugFormat("Unloading rec source {0}", name);

            using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
            {
                if (!m_recSources.ContainsKey(name))
                {
                    Error error = new Error(
                        errorCode: ErrorCodes.NoSuchRecSource,
                        message: string.Format("No rec source called \"{0}\" is loaded.", name));

                    throw new RecServiceErrorException(error);
                }
                else
                {
                    m_recSources.Remove(name);
                    m_recSourceFactories.Remove(name);
                }
            }

            Logging.Log.InfoFormat("Rec source {0} unloaded.", name);

            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }

        public string GetRecSourceType(string recSourceName)
        {
            recSourceName.ThrowIfNull("name");
            bool enteredLock;
            const int lockTimeoutInMs = 3000;
            using (m_recSourcesLock.ScopedReadLock(lockTimeoutInMs, out enteredLock))
            {
                // If we couldn't get a read lock within 3 seconds, a reload/retrain is probably going on
                if (!enteredLock)
                {
                    Error error = new Error(errorCode: ErrorCodes.Maintenance,
                        message: "The rec service is currently undergoing maintenance and cannot respond to rec requests.");
                    throw new RecServiceErrorException(error);
                }

                if (!m_recSources.ContainsKey(recSourceName))
                {
                    Error error = new Error(errorCode: ErrorCodes.NoSuchRecSource,
                        message: string.Format("No rec source called \"{0}\" is loaded.", recSourceName));
                    throw new RecServiceErrorException(error);
                }

                return m_recSources[recSourceName].RecSourceType;
            }
        }

        public void ReloadTrainingData(ReloadBehavior behavior, bool finalize)
        {
            switch (behavior)
            {
                case ReloadBehavior.LowMemory:
                    ReloadTrainingDataLowMemory(finalize);
                    break;
                case ReloadBehavior.HighAvailability:
                    ReloadTrainingDataHighAvailability(finalize);
                    break;
                default:
                    throw new Exception(string.Format("Unexpected ReloadBehavior: {0}", behavior));
            }
        }

        private void ReloadTrainingDataLowMemory(bool finalize)
        {
            using (var trainingDataWriteLock = m_trainingDataLock.ScopedWriteLock())
            using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
            {
                Logging.Log.Info("Reloading training data and prerequisites and retraining rec sources. Rec sources will not be available until retraining all rec sources is complete.");
                Stopwatch totalTimer = Stopwatch.StartNew();

                m_recSources.Clear();
                m_trainingData = null;
                m_usernames = null;
                m_animes = null;
                m_prereqs = null;
                m_finalized = false;

                GC.Collect();
                Logging.Log.Info("Rec sources cleared.");
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

                Stopwatch timer = Stopwatch.StartNew();

                // Load new training data
                // If this throws an error, m_trainingData is left null. Methods that use m_trainingData should check it for null.
                using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
                {
                    Logging.Log.Debug("Created training data loader.");

                    m_trainingData = malTrainingDataLoader.LoadMalTrainingData();
                    m_usernames = GetUsernamesFromTrainingData(m_trainingData);
                    m_animes = m_trainingData.Animes;
                    timer.Stop();

                    Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                        m_trainingData.Users.Count, m_trainingData.Animes.Count,
                        m_trainingData.Users.Keys.Sum(userId => m_trainingData.Users[userId].Entries.Count),
                        timer.Elapsed);

                    timer.Restart();
                    m_prereqs = malTrainingDataLoader.LoadPrerequisites();
                    timer.Stop();

                    int numPrereqs = m_prereqs.Values.Sum(prereqList => prereqList.Count);
                    Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                        numPrereqs, m_prereqs.Count, timer.Elapsed);
                }

                GC.Collect();
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

                // Then retrain all loaded rec sources.
                // ToList() so we can unload a rec source as we iterate if it errors while training.
                foreach (string recSourceName in m_recSourceFactories.Keys.ToList())
                {
                    ITrainableJsonRecSource recSource = m_recSourceFactories[recSourceName]();
                    try
                    {
                        Logging.Log.InfoFormat("Retraining rec source {0} ({1}).", recSourceName, recSource);
                        timer.Restart();

                        recSource.Train(m_trainingData, m_usernames);
                        m_recSources[recSourceName] = recSource;

                        timer.Stop();
                        Logging.Log.InfoFormat("Trained rec source {0} ({1}). Took {2}.", recSourceName, recSource, timer.Elapsed);
                        GC.Collect();
                        Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.ErrorFormat("Error retraining rec source {0} ({1}): {2} Unloading it.",
                            ex, recSourceName, recSource, ex.Message);
                        m_recSourceFactories.Remove(recSourceName);
                    }
                }

                if (finalize)
                {
                    m_trainingData = null;
                    m_usernames = null;
                    m_finalized = true;
                    Logging.Log.Info("Finalized rec sources.");
                }

                totalTimer.Stop();
                Logging.Log.InfoFormat("All rec sources retrained with the latest data. Total time: {0}", totalTimer.Elapsed);
            }

            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }

        private void ReloadTrainingDataHighAvailability(bool finalize)
        {
            Logging.Log.Info("Reloading training data and retraining rec sources. Rec sources will remain available.");
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            Stopwatch timer = Stopwatch.StartNew();
            Stopwatch totalTimer = Stopwatch.StartNew();

            // Load new training data
            MalTrainingData newData;
            IDictionary<int, string> newUsernames;
            IDictionary<int, IList<int>> newPrereqs;
            using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                Logging.Log.Debug("Created training data loader.");

                newData = malTrainingDataLoader.LoadMalTrainingData();
                newUsernames = GetUsernamesFromTrainingData(newData);

                timer.Stop();
                Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                    newData.Users.Count, newData.Animes.Count,
                    newData.Users.Keys.Sum(userId => newData.Users[userId].Entries.Count),
                    timer.Elapsed);

                timer.Restart();
                newPrereqs = malTrainingDataLoader.LoadPrerequisites();
                timer.Stop();

                int numPrereqs = newPrereqs.Values.Sum(prereqList => prereqList.Count);
                Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                    numPrereqs, newPrereqs.Count, timer.Elapsed);
            }

            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            using (var trainingDataWriteLock = m_trainingDataLock.ScopedWriteLock())
            using (var recSourcesUpgradeableLock = m_recSourcesLock.ScopedUpgradeableReadLock())
            {
                // clone the json rec sources without the training state and train each one with the new data.
                Dictionary<string, ITrainableJsonRecSource> newRecSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, Func<ITrainableJsonRecSource>> newRecSourceFactories = new Dictionary<string, Func<ITrainableJsonRecSource>>(m_recSourceFactories, StringComparer.OrdinalIgnoreCase);
                foreach (string recSourceName in newRecSourceFactories.Keys)
                {
                    ITrainableJsonRecSource recSource = newRecSourceFactories[recSourceName]();
                    Logging.Log.InfoFormat("Retraining rec source {0} ({1}).", recSourceName, recSource);
                    timer.Restart();
                    try
                    {
                        recSource.Train(newData, newUsernames);
                        timer.Stop();
                        Logging.Log.InfoFormat("Trained rec source {0} ({1}). Took {2}.", recSourceName, recSource, timer.Elapsed);
                        newRecSources[recSourceName] = recSource;

                        GC.Collect();
                        Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.ErrorFormat("Error retraining rec source {0} ({1}): {2} Unloading it.",
                            ex, recSourceName, recSource, ex.Message);
                        newRecSourceFactories.Remove(recSourceName);
                    }
                }

                // Swap in the newly trained rec sources.
                using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
                {
                    m_recSources = newRecSources;
                    m_recSourceFactories = newRecSourceFactories;

                    m_animes = newData.Animes;
                    m_prereqs = newPrereqs;

                    if (finalize)
                    {
                        m_trainingData = null;
                        m_usernames = null;
                        m_finalized = true;
                        Logging.Log.Info("Finalized rec sources.");
                    }
                    else
                    {
                        m_trainingData = newData;
                        m_usernames = newUsernames;
                        m_finalized = false;
                    }
                }
            }

            totalTimer.Stop();
            Logging.Log.InfoFormat("All rec sources retrained with the latest data. Total time: {0}", totalTimer.Elapsed);

            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }

        public GetMalRecsResponse GetMalRecs(GetMalRecsRequest request)
        {
            request.AssertArgumentNotNull("Payload");
            request.RecSourceName.AssertArgumentNotNull("Payload.RecSourceName");
            request.AnimeList.AssertArgumentNotNull("Payload.AnimeList");
            request.AnimeList.Entries.AssertArgumentNotNull("Payload.AnimeList.Entries");

            if (request.TargetScore == null && request.TargetFraction == null)
            {
                Error error = new Error(ErrorCodes.InvalidArgument, "Payload.TargetScore or Payload.TargetFraction must be set.");
                throw new RecServiceErrorException(error);
            }

            string targetScoreString;
            if (request.TargetFraction != null)
            {
                targetScoreString = request.TargetFraction.Value.ToString("P2");
            }
            else
            {
                targetScoreString = request.TargetScore.Value.ToString();
            }

            Logging.Log.InfoFormat("Request for {0} MAL recs using rec source {1}. User has {2} anime list entries. Target score is {3}.",
                request.NumRecsDesired, request.RecSourceName, request.AnimeList.Entries.Count, targetScoreString);

            // Acquire read lock on rec sources
            bool enteredLock;
            const int lockTimeoutInMs = 3000;
            using (var trainingDataReadLock = m_trainingDataLock.ScopedReadLock(lockTimeoutInMs, out enteredLock))
            {
                // If we couldn't get a read lock within 3 seconds, a reload/retrain is probably going on
                if (!enteredLock)
                {
                    Error error = new Error(errorCode: ErrorCodes.Maintenance,
                        message: "The rec service is currently undergoing maintenance and cannot respond to rec requests.");
                    throw new RecServiceErrorException(error);
                }

                using (var recSourcesReadLock = m_recSourcesLock.ScopedReadLock())
                {
                    // Get rec source by name
                    if (!m_recSources.ContainsKey(request.RecSourceName))
                    {
                        Error error = new Error(errorCode: ErrorCodes.NoSuchRecSource,
                            message: string.Format("No rec source called \"{0}\" is loaded.", request.RecSourceName));
                        throw new RecServiceErrorException(error);
                    }
                    ITrainableJsonRecSource recSource = m_recSources[request.RecSourceName];

                    // Convert DTO anime list to RecEngine anime list

                    Dictionary<int, AnimeRecs.RecEngine.MAL.MalListEntry> entries = new Dictionary<int, RecEngine.MAL.MalListEntry>();
                    foreach (AnimeRecs.RecService.DTO.MalListEntry dtoEntry in request.AnimeList.Entries)
                    {
                        AnimeRecs.RecEngine.MAL.MalListEntry recEngineEntry = new RecEngine.MAL.MalListEntry(dtoEntry.Rating, dtoEntry.Status, dtoEntry.NumEpisodesWatched);
                        entries[dtoEntry.MalAnimeId] = recEngineEntry;
                    }
                    MalUserListEntries animeList = new MalUserListEntries(ratings: entries, animes: m_animes,
                        malUsername: null, prerequisites: m_prereqs);

                    Stopwatch timer = Stopwatch.StartNew();
                    GetMalRecsResponse response = recSource.GetRecommendations(animeList, request);
                    timer.Stop();

                    Logging.Log.InfoFormat("Got recommendations from rec source {0}. Took {1}.", request.RecSourceName, timer.Elapsed);
                    return response;
                }
            }
        }

        public void FinalizeRecSources()
        {
            using (var trainingDataWriteLock = m_trainingDataLock.ScopedWriteLock())
            {
                m_trainingData = null;
                m_usernames = null;
                m_finalized = true;
            }
            GC.Collect();
            Logging.Log.Info("Finalized rec sources.");
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }

        public void Dispose()
        {
            m_trainingDataLock.Dispose();
            m_recSourcesLock.Dispose();
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.