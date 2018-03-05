using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;
using AnimeRecs.DAL;
using System.Diagnostics;
using System.Threading.Tasks;
using AnimeRecs.Utils;

namespace AnimeRecs.RecService
{
    /// <summary>
    /// Stores the state of the recommendation service, including rec sources. All public methods and properties are thread-safe.
    /// </summary>
    internal class RecServiceState
    {
        // When locking, always lock in the order the members are listed here.

        private bool m_finalized = false;
        private MalTrainingData m_trainingData;
        private IDictionary<int, string> m_usernames; // Always update this when updating m_trainingData. Is null when rec sources are finalized.
        private IDictionary<int, RecEngine.MAL.MalAnime> m_animes; // Always update this when updating m_trainingData. Remains when rec sources are finalized.
        private IDictionary<int, IList<int>> m_prereqs; // Update this when reloading.
        private AsyncUpgradeableReaderWriterLock m_trainingDataLockAsync;

        private Dictionary<string, Func<ITrainableJsonRecSource>> m_recSourceFactories = new Dictionary<string, Func<ITrainableJsonRecSource>>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, ITrainableJsonRecSource> m_recSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
        private AsyncUpgradeableReaderWriterLock m_recSourcesLockAsync;

        private HashSet<string> m_pendingRecSources = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private AsyncUpgradeableReaderWriterLock m_pendingRecSourcesLockAsync;

        private IMalTrainingDataLoaderFactory m_trainingDataLoaderFactory;

        // Thread-safe because it doesn't change after construction
        public IDictionary<string, Type> JsonRecSourceTypes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingDataLoaderFactory">Must be thread-safe.</param>
        public RecServiceState(IMalTrainingDataLoaderFactory trainingDataLoaderFactory, MalTrainingData trainingData, IDictionary<int, IList<int>> prereqs)
        {
            m_trainingDataLoaderFactory = trainingDataLoaderFactory;
            JsonRecSourceTypes = GetJsonRecSourceTypes();
            m_trainingData = trainingData;
            m_usernames = GetUsernamesFromTrainingData(m_trainingData);
            m_animes = m_trainingData.Animes;
            m_prereqs = prereqs;
            m_trainingDataLockAsync = new AsyncUpgradeableReaderWriterLock();
            m_recSourcesLockAsync = new AsyncUpgradeableReaderWriterLock();
            m_pendingRecSourcesLockAsync = new AsyncUpgradeableReaderWriterLock();
        }

        private static IDictionary<string, Type> GetJsonRecSourceTypes()
        {
            Logging.Log.Debug("Searching for types that implement ITrainableJsonRecSource and have at least one [JsonRecSource] attribute.");

            Type[] typesInThisAssembly = typeof(RecServiceState).GetTypeInfo().Assembly.GetTypes();
            Type jsonRecSourceInterface = typeof(ITrainableJsonRecSource);
            Type jsonRecSourceAttribute = typeof(JsonRecSourceAttribute);

            Dictionary<string, Type> jsonRecSourceTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (Type type in typesInThisAssembly)
            {
                TypeInfo typeInfo = type.GetTypeInfo();
                if (jsonRecSourceInterface.IsAssignableFrom(type) && typeInfo.IsClass && !typeInfo.IsAbstract)
                {
                    List<string> recSourceTypesDeclaredFor = new List<string>();

                    IEnumerable<JsonRecSourceAttribute> jsonRecSourceAttributes = typeInfo.GetCustomAttributes(jsonRecSourceAttribute, inherit: false).Cast<JsonRecSourceAttribute>();
                    foreach (JsonRecSourceAttribute attribute in jsonRecSourceAttributes)
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

        public Task LoadRecSourceAsync(LoadRecSourceRequest recSourceConfig, CancellationToken cancellationToken)
        {
            if (JsonRecSourceTypes.ContainsKey(recSourceConfig.Type))
            {
                Type jsonRecSourceType = JsonRecSourceTypes[recSourceConfig.Type];

                // recSourceConfig's static type is LoadRecSourceRequest.
                // Its real type will be something like LoadRecSourceRequest<AverageScoreRecSourceParams> thanks to the custom JsonConverter.
                // A json rec source is expected to have one or more constructors taking types derived from LoadRecSourceRequest.
                Func<ITrainableJsonRecSource> recSourceFactory = () => (ITrainableJsonRecSource)(Activator.CreateInstance(jsonRecSourceType, recSourceConfig));

                return LoadRecSourceAsync(recSourceFactory, recSourceConfig.Name, recSourceConfig.ReplaceExisting, cancellationToken);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("{0} is not a valid rec source type.", recSourceConfig.Name));
            }
        }

        public async Task LoadRecSourceAsync(Func<ITrainableJsonRecSource> recSourceFactory, string name, bool replaceExisting, CancellationToken cancellationToken)
        {
            // Acquire read lock on current list, write lock on pending list
            // If name already exists on current list and replaceExisting = false, throw.
            // If name already exists on pending list, throw.
            // Otherwise, add name to pending list, release locks, and proceed.

            using (var recSourcesReadLock = await m_recSourcesLockAsync.EnterReadLockAsync(cancellationToken).ConfigureAwait(false))
            using (var pendingRecSourcesWriteLock = await m_pendingRecSourcesLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
            {
                if (m_recSources.ContainsKey(name) && !replaceExisting)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Unknown,
                        message: string.Format("A recommendation source with the name \"{0}\" already exists.", name)));
                }
                if (m_pendingRecSources.Contains(name))
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Unknown,
                        message: string.Format("A recommendation source with the name \"{0}\" is currently being trained.", name)));
                }

                m_pendingRecSources.Add(name);
            }

            try
            {
                // Need to hold read lock on training data while training so that a retrain can't happen while we're training here.
                // Rec sources must be trained with the current m_trainingData, not an old version.
                using (var trainingDataReadLock = await m_trainingDataLockAsync.EnterReadLockAsync(cancellationToken).ConfigureAwait(false))
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

                    ITrainableJsonRecSource recSource = recSourceFactory();
                    Logging.Log.InfoFormat("Training rec source with name \"{0}\", replaceExisting={1}: {2}", name, replaceExisting, recSource);
                    Stopwatch timer = Stopwatch.StartNew();

                    recSource.Train(m_trainingData, m_usernames, cancellationToken);
                    timer.Stop();
                    Logging.Log.InfoFormat("Trained rec source {0}. Took {1}", name, timer.Elapsed);

                    using (var recSourcesWriteLock = await m_recSourcesLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
                    using (var pendingRecSourcesWriteLock = await m_pendingRecSourcesLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
                    {
                        m_recSources[name] = recSource;
                        m_recSourceFactories[name] = recSourceFactory;
                        m_pendingRecSources.Remove(name);
                    }
                }
            }
            catch (Exception)
            {
                m_pendingRecSources.Remove(name);
                throw;
            }

            Logging.Log.InfoFormat("Loaded rec source {0}.", name);
            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
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

        public async Task UnloadRecSourceAsync(string name, CancellationToken cancellationToken)
        {
            if (name == null) throw new ArgumentNullException("name");
            Logging.Log.DebugFormat("Unloading rec source {0}", name);

            using (var recSourcesWriteLock = await m_recSourcesLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
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

        public async Task<string> GetRecSourceTypeAsync(string recSourceName, CancellationToken cancellationToken)
        {
            if (recSourceName == null) throw new ArgumentNullException("recSourceName");
            const int lockTimeoutInSeconds = 3;
            using (CancellationTokenSource lockTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(lockTimeoutInSeconds)))
            using (CancellationTokenSource lockCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, lockTimeout.Token))
            {
                bool gotLock = false;
                try
                {
                    using (var recSourcesLock = await m_recSourcesLockAsync.EnterReadLockAsync(lockCancel.Token).ConfigureAwait(false))
                    {
                        gotLock = true;
                        if (!m_recSources.ContainsKey(recSourceName))
                        {
                            Error error = new Error(errorCode: ErrorCodes.NoSuchRecSource,
                                message: string.Format("No rec source called \"{0}\" is loaded.", recSourceName));
                            throw new RecServiceErrorException(error);
                        }

                        return m_recSources[recSourceName].RecSourceType;
                    }
                }
                catch (OperationCanceledException)
                {
                    // If we couldn't get a read lock within 3 seconds, a reload/retrain is probably going on
                    if (!gotLock)
                    {
                        Error error = new Error(errorCode: ErrorCodes.Maintenance,
                                message: "The rec service is currently undergoing maintenance and cannot respond to rec requests.");
                        throw new RecServiceErrorException(error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public Task ReloadTrainingDataAsync(ReloadBehavior behavior, bool finalize, CancellationToken cancellationToken)
        {
            switch (behavior)
            {
                case ReloadBehavior.LowMemory:
                    return ReloadTrainingDataLowMemoryAsync(finalize, cancellationToken);
                case ReloadBehavior.HighAvailability:
                    return ReloadTrainingDataHighAvailabilityAsync(finalize, cancellationToken);
                default:
                    throw new Exception(string.Format("Unexpected ReloadBehavior: {0}", behavior));
            }
        }

        private async Task ReloadTrainingDataLowMemoryAsync(bool finalize, CancellationToken cancellationToken)
        {
            using (var trainingDataWriteLock = await m_trainingDataLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
            using (var recSourcesWriteLock = await m_recSourcesLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
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

                // Load new training data
                // If this throws an error, m_trainingData is left null. Methods that use m_trainingData should check it for null.
                using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
                using (CancellationTokenSource faultCanceler = new CancellationTokenSource())
                using (CancellationTokenSource faultOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, faultCanceler.Token))
                {
                    Stopwatch trainingDataTimer = Stopwatch.StartNew();

                    CancellableTask<MalTrainingData> trainingDataTask = new CancellableTask<MalTrainingData>(
                        malTrainingDataLoader.LoadMalTrainingDataAsync(faultOrUserCancel.Token), faultCanceler);

                    Task trainingDataTimerTask = trainingDataTask.Task.ContinueWith(task =>
                    {
                        trainingDataTimer.Stop();
                        Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                            task.Result.Users.Count, task.Result.Animes.Count,
                            task.Result.Users.Keys.Sum(userId => task.Result.Users[userId].Entries.Count),
                            trainingDataTimer.Elapsed);
                    },
                    cancellationToken, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled
                    | TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);

                    Stopwatch prereqsTimer = Stopwatch.StartNew();

                    CancellableTask<IDictionary<int, IList<int>>> prereqsTask = new CancellableTask<IDictionary<int, IList<int>>>(
                        malTrainingDataLoader.LoadPrerequisitesAsync(faultOrUserCancel.Token), faultCanceler);

                    Task prereqsTimerTask = prereqsTask.Task.ContinueWith(task =>
                    {
                        prereqsTimer.Stop();
                        int numPrereqs = task.Result.Values.Sum(prereqList => prereqList.Count);
                        Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                            numPrereqs, task.Result.Count, prereqsTimer.Elapsed);
                    },
                    cancellationToken, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled
                    | TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);

                    await AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(trainingDataTask, prereqsTask).ConfigureAwait(false);

                    m_trainingData = trainingDataTask.Task.Result;
                    m_usernames = GetUsernamesFromTrainingData(m_trainingData);
                    m_animes = m_trainingData.Animes;

                    m_prereqs = prereqsTask.Task.Result;

                    await trainingDataTimerTask.ConfigureAwait(false);
                    await prereqsTimerTask.ConfigureAwait(false);
                }

                GC.Collect();
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

                // Then retrain all loaded rec sources.

                if (m_recSourceFactories.Count == 0)
                {
                    Logging.Log.Info("No rec sources to retrain.");
                }
                else
                {
                    Logging.Log.Info("Retraining rec sources.");

                    object recSourcesLockAndMemFence = new object();

                    List<Task> recSourceTrainTasksList = new List<Task>();

                    // ToList() so we can unload a rec source as we iterate if it errors while training.
                    foreach (string recSourceNameLoopVar in m_recSourceFactories.Keys.ToList())
                    {
                        string recSourceName = recSourceNameLoopVar; // avoid capturing the loop var
                        ITrainableJsonRecSource recSource = m_recSourceFactories[recSourceName]();

                        Task recSourceTrainTask = Task.Run(() =>
                        {
                            Logging.Log.InfoFormat("Retraining rec source {0} ({1}).", recSourceName, recSource);
                            Stopwatch trainTimer = Stopwatch.StartNew();

                            try
                            {
                                recSource.Train(m_trainingData, m_usernames, cancellationToken);
                                trainTimer.Stop();
                                Logging.Log.InfoFormat("Trained rec source {0} ({1}). Took {2}.", recSourceName, recSource, trainTimer.Elapsed);

                                lock (recSourcesLockAndMemFence)
                                {
                                    m_recSources[recSourceName] = recSource;
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                Logging.Log.InfoFormat("Canceled while retraining rec source {0} ({1}). Unloading it.", recSourceName, recSource);
                                lock (recSourcesLockAndMemFence)
                                {
                                    m_recSourceFactories.Remove(recSourceName);
                                }
                                throw;
                            }
                            catch (Exception ex)
                            {
                                Logging.Log.ErrorFormat("Error retraining rec source {0} ({1}): {2} Unloading it.",
                                    ex, recSourceName, recSource, ex.Message);
                                lock (recSourcesLockAndMemFence)
                                {
                                    m_recSourceFactories.Remove(recSourceName);
                                }
                            }
                        }, cancellationToken);

                        recSourceTrainTasksList.Add(recSourceTrainTask);
                    }

                    // Wait for all to complete or cancellation. There should not be any exceptions other than OperationCanceledException.
                    await Task.WhenAll(recSourceTrainTasksList).ConfigureAwait(false);

                    lock (recSourcesLockAndMemFence)
                    {
                        ; // just for the fence
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

        private async Task ReloadTrainingDataHighAvailabilityAsync(bool finalize, CancellationToken cancellationToken)
        {
            using (var trainingDataUpgradeableLock = await m_trainingDataLockAsync.EnterUpgradeableReadLockAsync(cancellationToken).ConfigureAwait(false))
            {
                Logging.Log.Info("Reloading training data and retraining rec sources. Rec sources will remain available.");
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

                Stopwatch totalTimer = Stopwatch.StartNew();

                // Load new training data
                MalTrainingData newData;
                IDictionary<int, string> newUsernames;
                IDictionary<int, IList<int>> newPrereqs;
                using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
                using (CancellationTokenSource faultCanceler = new CancellationTokenSource())
                using (CancellationTokenSource faultOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, faultCanceler.Token))
                {
                    Stopwatch trainingDataTimer = Stopwatch.StartNew();

                    CancellableTask<MalTrainingData> trainingDataTask = new CancellableTask<MalTrainingData>(
                        malTrainingDataLoader.LoadMalTrainingDataAsync(faultOrUserCancel.Token), faultCanceler);

                    Task trainingDataTimerTask = trainingDataTask.Task.ContinueWith(task =>
                    {
                        trainingDataTimer.Stop();
                        Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                            task.Result.Users.Count, task.Result.Animes.Count,
                            task.Result.Users.Keys.Sum(userId => task.Result.Users[userId].Entries.Count),
                            trainingDataTimer.Elapsed);
                    },
                    cancellationToken, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled
                    | TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);

                    Stopwatch prereqsTimer = Stopwatch.StartNew();

                    CancellableTask<IDictionary<int, IList<int>>> prereqsTask = new CancellableTask<IDictionary<int, IList<int>>>(
                        malTrainingDataLoader.LoadPrerequisitesAsync(faultOrUserCancel.Token), faultCanceler);

                    Task prereqsTimerTask = prereqsTask.Task.ContinueWith(task =>
                    {
                        prereqsTimer.Stop();
                        int numPrereqs = task.Result.Values.Sum(prereqList => prereqList.Count);
                        Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                            numPrereqs, task.Result.Count, prereqsTimer.Elapsed);
                    },
                    cancellationToken, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled
                    | TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);

                    await AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(trainingDataTask, prereqsTask);

                    newData = trainingDataTask.Task.Result;
                    newUsernames = GetUsernamesFromTrainingData(newData);

                    newPrereqs = prereqsTask.Task.Result;

                    await trainingDataTimerTask.ConfigureAwait(false);
                    await prereqsTimerTask.ConfigureAwait(false);
                }

                GC.Collect();
                Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

                using (var recSourcesUpgradeableLock = await m_recSourcesLockAsync.EnterUpgradeableReadLockAsync(cancellationToken).ConfigureAwait(false))
                {
                    // clone the json rec sources without the training state and train each one with the new data.
                    Dictionary<string, ITrainableJsonRecSource> newRecSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, Func<ITrainableJsonRecSource>> newRecSourceFactories = new Dictionary<string, Func<ITrainableJsonRecSource>>(m_recSourceFactories, StringComparer.OrdinalIgnoreCase);

                    if (m_recSourceFactories.Count == 0)
                    {
                        Logging.Log.Info("No rec sources to retrain.");
                    }
                    else
                    {
                        Logging.Log.Info("Retraining rec sources.");

                        object newRecSourcesLockAndMemFence = new object();

                        List<Task> recSourceTrainTasksList = new List<Task>();

                        // ToList() so we can unload a rec source as we iterate if it errors while training.
                        foreach (string recSourceNameLoopVar in m_recSourceFactories.Keys.ToList())
                        {
                            string recSourceName = recSourceNameLoopVar; // avoid capturing the loop var
                            ITrainableJsonRecSource recSource = newRecSourceFactories[recSourceName]();

                            Task recSourceTrainTask = Task.Run(() =>
                            {
                                Logging.Log.InfoFormat("Retraining rec source {0} ({1}).", recSourceName, recSource);
                                Stopwatch trainTimer = Stopwatch.StartNew();

                                try
                                {
                                    recSource.Train(newData, newUsernames, cancellationToken);
                                    trainTimer.Stop();
                                    Logging.Log.InfoFormat("Trained rec source {0} ({1}). Took {2}.", recSourceName, recSource, trainTimer.Elapsed);
                                    lock (newRecSourcesLockAndMemFence)
                                    {
                                        newRecSources[recSourceName] = recSource;
                                    }
                                }
                                catch (OperationCanceledException)
                                {
                                    Logging.Log.InfoFormat("Canceled while retraining rec source {0} ({1}).", recSourceName, recSource);
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    Logging.Log.ErrorFormat("Error retraining rec source {0} ({1}): {2} Unloading it.",
                                        ex, recSourceName, recSource, ex.Message);

                                    lock (newRecSourcesLockAndMemFence)
                                    {
                                        newRecSourceFactories.Remove(recSourceName);
                                    }
                                }
                            }, cancellationToken);

                            recSourceTrainTasksList.Add(recSourceTrainTask);
                        }

                        // Wait for all to complete or cancellation. There should not be any exceptions other than OperationCanceledException.
                        await Task.WhenAll(recSourceTrainTasksList);

                        lock (newRecSourcesLockAndMemFence)
                        {
                            ; // just for the fence
                        }
                    }

                    // Swap in the newly trained rec sources.
                    using (var trainingDataWriteLock = await m_trainingDataLockAsync.UpgradeToWriteLock(cancellationToken).ConfigureAwait(false))
                    using (var recSourcesWriteLock = await m_recSourcesLockAsync.UpgradeToWriteLock(cancellationToken).ConfigureAwait(false))
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
            }

            GC.Collect();
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }

        public async Task<GetMalRecsResponse> GetMalRecsAsync(GetMalRecsRequest request, CancellationToken cancellationToken)
        {
            request.AssertArgumentNotNull("request");
            request.RecSourceName.AssertArgumentNotNull("request.RecSourceName");
            request.AnimeList.AssertArgumentNotNull("request.AnimeList");
            request.AnimeList.Entries.AssertArgumentNotNull("request.AnimeList.Entries");

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
            const int lockTimeoutInSeconds = 3;
            using (CancellationTokenSource lockTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(lockTimeoutInSeconds)))
            using (CancellationTokenSource lockCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, lockTimeout.Token))
            {
                bool gotLock = false;
                try
                {
                    using (var recSourcesLock = await m_recSourcesLockAsync.EnterReadLockAsync(lockCancel.Token).ConfigureAwait(false))
                    {
                        gotLock = true;
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
                        GetMalRecsResponse response = recSource.GetRecommendations(animeList, request, cancellationToken);
                        timer.Stop();

                        Logging.Log.InfoFormat("Got recommendations from rec source {0}. Took {1}.", request.RecSourceName, timer.Elapsed);
                        return response;
                    }
                }
                catch (OperationCanceledException)
                {
                    // If we couldn't get a read lock within 3 seconds, a reload/retrain is probably going on
                    if (!gotLock)
                    {
                        Error error = new Error(errorCode: ErrorCodes.Maintenance,
                                message: "The rec service is currently undergoing maintenance and cannot respond to rec requests.");
                        throw new RecServiceErrorException(error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public async Task FinalizeRecSourcesAsync(CancellationToken cancellationToken)
        {
            using (var trainingDataWriteLock = await m_trainingDataLockAsync.EnterWriteLockAsync(cancellationToken).ConfigureAwait(false))
            {
                m_trainingData = null;
                m_usernames = null;
                m_finalized = true;
            }
            GC.Collect();
            Logging.Log.Info("Finalized rec sources.");
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));
        }
    }
}
