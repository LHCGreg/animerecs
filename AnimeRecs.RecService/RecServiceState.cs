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

        private MalTrainingData m_trainingData;
        private ReaderWriterLockSlim m_trainingDataLock;
        private Dictionary<string, ITrainableJsonRecSource> m_recSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
        private ReaderWriterLockSlim m_recSourcesLock;

        private IMalTrainingDataLoaderFactory m_trainingDataLoaderFactory;

        public IDictionary<string, Type> JsonRecSourceTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingDataLoaderFactory">Must be thread-safe.</param>
        public RecServiceState(IMalTrainingDataLoaderFactory trainingDataLoaderFactory)
        {
            m_trainingDataLoaderFactory = trainingDataLoaderFactory;
            JsonRecSourceTypes = GetJsonRecSourceTypes();
            m_trainingData = LoadTrainingDataOnInit(trainingDataLoaderFactory);
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
                    foreach(JsonRecSourceAttribute attribute in jsonRecSourceAttributes.Select(attributeObj => (JsonRecSourceAttribute)attributeObj))
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

        private MalTrainingData LoadTrainingDataOnInit(IMalTrainingDataLoaderFactory trainingDataLoaderFactory)
        {
            Logging.Log.Info("Loading training data.");
            Stopwatch timer = Stopwatch.StartNew();
            MalTrainingData trainingData;

            using (IMalTrainingDataLoader trainingDataLoader = trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                Logging.Log.Debug("Created training data loader.");
                trainingData = trainingDataLoader.LoadMalTrainingData();
                timer.Stop();
            }

            Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                trainingData.Users.Count, trainingData.Animes.Count,
                trainingData.Users.Keys.Sum(userId => trainingData.Users[userId].Entries.Count),
                timer.Elapsed);

            return trainingData;
        }

        public void LoadRecSource(ITrainableJsonRecSource recSource, string name, bool replaceExisting)
        {
            Logging.Log.InfoFormat("Loading rec source with name \"{0}\", replaceExisting={1}: {2}", name, replaceExisting, recSource);

            // Need to hold read lock on training data while training so that a retrain can't happen while we're training here.
            // Rec sources must be trained with the current m_trainingData, not an old version.
            using (var trainingDataReadLock = m_trainingDataLock.ScopedReadLock())
            using (var recSourcesUpgradeableReadLock = m_recSourcesLock.ScopedUpgradeableReadLock())
            {
                if (m_recSources.ContainsKey(name) && !replaceExisting)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Unknown,
                        message: string.Format("A recommendation source with the name \"{0}\" already exists.", name)));
                }

                Logging.Log.InfoFormat("Training rec source {0}.", name);
                Stopwatch timer = Stopwatch.StartNew();

                recSource.Train(m_trainingData);

                timer.Stop();
                Logging.Log.InfoFormat("Trained rec source {0}. Took {1}", name, timer.Elapsed);

                using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
                {
                    m_recSources[name] = recSource;
                }
                Logging.Log.InfoFormat("Loaded rec source {0}.", name);

                GC.Collect();
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
            }
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

        public void ReloadTrainingData()
        {
            Logging.Log.Info("Reloading training data and retraining rec sources.");
            Stopwatch timer = Stopwatch.StartNew();
            Stopwatch totalTimer = Stopwatch.StartNew();
            
            MalTrainingData newData;
            // Load new training data first
            using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                Logging.Log.Debug("Created training data loader.");
                newData = malTrainingDataLoader.LoadMalTrainingData();
                timer.Stop();
            }

            Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                m_trainingData.Users.Count, m_trainingData.Animes.Count,
                m_trainingData.Users.Keys.Sum(userId => m_trainingData.Users[userId].Entries.Count),
                timer.Elapsed);

            // Then swap out training data and retrain all loaded rec sources
            using (var trainingDataWriteLock = m_trainingDataLock.ScopedWriteLock())
            using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
            {
                // ToList() so we can unload a rec source as we iterate if it errors while training.
                foreach(string recSourceName in m_recSources.Keys.ToList()) 
                {
                    ITrainableJsonRecSource recSource = m_recSources[recSourceName];
                    try
                    {
                        Logging.Log.InfoFormat("Retraining rec source {0} ({1}).", recSourceName, recSource);
                        timer.Restart();

                        // XXX: Recommendation requests block while retraining the rec sources.
                        // This could be in the seconds to minutes range depending on the size of the dataset and how
                        // computationally intensive the rec sources are.
                        recSource.Train(newData);

                        timer.Stop();
                        Logging.Log.InfoFormat("Trained rec source {0} ({1}). Took {2}.", recSourceName, recSource, timer.Elapsed);
                    }
                    catch (Exception ex)
                    {
                        Logging.Log.ErrorFormat("Error retraining rec source {0} ({1}): {2} Unloading it.",
                            ex, recSourceName, recSource, ex.Message);
                        m_recSources.Remove(recSourceName);
                    }
                }

                m_trainingData = newData;
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
            using (var recSourcesReadLock = m_recSourcesLock.ScopedReadLock(lockTimeoutInMs, out enteredLock))
            {
                // If we couldn't get a read lock within 3 seconds, a reload/retrain is probably going on
                if(!enteredLock)
                {
                    Error error = new Error(errorCode: ErrorCodes.Maintenance,
                        message: "The rec service is currently undergoing maintenance and cannot respond to rec requests.");
                    throw new RecServiceErrorException(error);
                }
                
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
                MalUserListEntries animeList = new MalUserListEntries(ratings: entries, animes: m_trainingData.Animes, malUsername: null);

                Stopwatch timer = Stopwatch.StartNew();
                GetMalRecsResponse response = recSource.GetRecommendations(animeList, request);
                timer.Stop();

                Logging.Log.InfoFormat("Got recommendations from rec source {0}. Took {1}.", request.RecSourceName, timer.Elapsed);
                return response;
            }
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