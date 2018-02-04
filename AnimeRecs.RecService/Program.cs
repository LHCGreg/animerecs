using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.Utils;

#if NETCORE
using System.Runtime.Loader;
using System.Reflection;
#endif

namespace AnimeRecs.RecService
{
    enum ExitCode
    {
        Success = 0,
        Error = 1
    }

    internal class Program
    {
        // Used for the SIGTERM handler, which must wait for all shutdown to complete before returning from the event handler.
        static ManualResetEventSlim Done = new ManualResetEventSlim(false);

        static object SigHandlerLock = new object();
        static bool ShuttingDown = false;

        static int Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            CommandLineArgs commandLine;
            Config config;

            try
            {
                commandLine = new CommandLineArgs(args);
                if (commandLine.ShowHelp)
                {
                    commandLine.DisplayHelp(Console.Out);
                    return (int)ExitCode.Success;
                }

                config = Config.LoadFromFile(commandLine.ConfigFile);

                if (config.LoggingConfigPath != null)
                {
                    Logging.SetUpLogging(config.LoggingConfigPath);
                }
                else
                {
                    Console.Error.WriteLine("No logging configuration file set. Logging to console.");
                    Logging.SetUpConsoleLogging();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Fatal error: {0}", ex, ex.Message);
                return (int)ExitCode.Error;
            }

            try
            {
                Logging.Log.DebugFormat("Command line args parsed. PortNumber={0}, ConfigFile={1}, ShowHelp={2}", commandLine.PortNumber, commandLine.ConfigFile, commandLine.ShowHelp);
                StartServiceAndWait(commandLine, config);
                return (int)ExitCode.Success;
            }
            catch (AggregateException aggEx)
            {
                AggregateException flattened = aggEx.Flatten();
                foreach (Exception ex in flattened.InnerExceptions)
                {
                    Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
                }

                return (int)ExitCode.Error;
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
                return (int)ExitCode.Error;
            }
            finally
            {
                Done.Set();
            }
        }

        // Can throw AggregateException and regular Exceptions.
        private static void StartServiceAndWait(CommandLineArgs commandLine, Config config)
        {
            WarmUpJsonSerializingInBackground();
            
            // When a request to stop the service is sent, cancel this cancellation token source.
            using (CancellationTokenSource serviceStopper = new CancellationTokenSource())
            {
                RegisterCancellationHandlers(serviceStopper);

                try
                {
                    StartServiceAndWaitThrowOpCanceled(commandLine, config, serviceStopper.Token);
                }
                catch (OperationCanceledException)
                {
                    Logging.Log.Info("Rec service shutdown complete.");
                }
            }
        }

        private static void WarmUpJsonSerializingInBackground()
        {
            Task.Run(action: WarmUpJsonSerializing);
        }

        private static void WarmUpJsonSerializing()
        {
            Logging.Log.Debug("Warming up JSON serialization.");
            Stopwatch timer = Stopwatch.StartNew();
            try
            {
                DTO.Optimization.WarmUpJsonSerializing();
                timer.Stop();
                Logging.Log.DebugFormat("Done warming up JSON serialization. Took {0}", timer.Elapsed);
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Error warming up JSON serialization: {0}", ex, ex.Message);
            }
        }

        static void RegisterCancellationHandlers(CancellationTokenSource serviceStopper)
        {
            // Set up how the service will be stopped.
            // Console.CancelKeyPress corresponds to pressing ctrl+c in the console on Windows
            // and to SIGINT on Linux, which can be sent in a console with ctrl+c or programtically or from a shell with kill.
            // assemblyLoadContext.Unloading corresponds to SIGTERM on Linux and nothing(?) on Windows.
            // AssemblyLoadContext is only on .net core, not .net 4.7.

            lock (SigHandlerLock)
            {
                // Don't need to wait for shutdown inside the event handler for SIGINT because the process doesn't die after the event handler returns.
                // The SIGTERM handler on the other hand...not sure but the process might die after the event handler returns.
                Console.CancelKeyPress += (sender, eventArgs) => { StopService(serviceStopper, waitForShutdown: false); eventArgs.Cancel = true; };
#if NETCORE
                var assemblyLoadContext = AssemblyLoadContext.GetLoadContext(typeof(Program).GetTypeInfo().Assembly);

                // Sleep for a bit after everything is shut down to give logs a chance to flush
                assemblyLoadContext.Unloading += context => { StopService(serviceStopper, waitForShutdown: true); Thread.Sleep(TimeSpan.FromMilliseconds(500)); };
#endif
            }
        }

        static void StopService(CancellationTokenSource serviceStopper, bool waitForShutdown)
        {
            lock (SigHandlerLock)
            {
                // If we're already in the process of shutting down, don't do anything (except wait for shutdown in SIGTERM handler).
                // Can't use serviceStopper.IsCancellationRequested because that cancellation token source could be
                // disposed in the middle of shutting down.
                if (!ShuttingDown)
                {
                    ShuttingDown = true;
                    Logging.Log.Info("Stopping rec service...");
                    serviceStopper.Cancel();
                }
            }

            if (waitForShutdown)
            {
                Done.Wait();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="serviceStopToken"></param>
        /// <exception cref="System.OperationCanceledException">Service stop has been requested.</exception>
        /// <exception cref="System.AggregateException"></exception>
        /// <exception cref="System.Exception"></exception>
        // This function is expected to synchronously start the service and wait for the cancellation token to be signaled,
        // indicating the service should stop. When that happens, it should throw OperationCanceledException
        private static void StartServiceAndWaitThrowOpCanceled(CommandLineArgs commandLine, Config config, CancellationToken serviceStopToken)
        {
            serviceStopToken.ThrowIfCancellationRequested();

            PgMalTrainingDataLoaderFactory trainingDataLoaderFactory = new PgMalTrainingDataLoaderFactory(config.ConnectionStrings.AnimeRecs);

            (MalTrainingData initialTrainingData, IDictionary<int, IList<int>> initialPrereqs) = LoadInitialData(trainingDataLoaderFactory, serviceStopToken);

            // TcpRecService constructor does not start the service and does not block on anything.
            using (TcpRecService recService = new TcpRecService(commandLine.PortNumber, trainingDataLoaderFactory, initialTrainingData, initialPrereqs))
            {
                // Load rec sources into the rec service before listening for connections.
                LoadRecSources(recService, config, serviceStopToken);

                // Start listening for connections. Does not block.
                recService.Start();
                Logging.Log.InfoFormat("Started listening on port {0}.", commandLine.PortNumber);

                // Wait for a service stop request to come in.
                serviceStopToken.WaitHandle.WaitOne();

                // Stop the service, letting connections drain for a few seconds after stopping listening for new connections.
                const int connectionDrainTimeoutInSeconds = 5;
                recService.Stop(TimeSpan.FromSeconds(connectionDrainTimeoutInSeconds));

                // Throw OperationCanceledException to notify caller that the service was stopped in an orderly manner.
                throw new OperationCanceledException(serviceStopToken);
            }
        }

        // Loads training data and prerequisites from the database in parallel and does not return until they are loaded.
        private static (MalTrainingData trainingData, IDictionary<int, IList<int>> prereqs) LoadInitialData(IMalTrainingDataLoaderFactory trainingDataLoaderFactory, CancellationToken serviceStopToken)
        {
            using (IMalTrainingDataLoader initialTrainingDataLoader = trainingDataLoaderFactory.GetTrainingDataLoader())
            using (CancellationTokenSource trainingDataOtherFaultOrCancellation = new CancellationTokenSource())
            using (CancellationTokenSource trainingDataCancel = CancellationTokenSource.CreateLinkedTokenSource(serviceStopToken, trainingDataOtherFaultOrCancellation.Token))
            using (CancellationTokenSource prereqsOtherFaultOrCancellation = new CancellationTokenSource())
            using (CancellationTokenSource prereqsCancel = CancellationTokenSource.CreateLinkedTokenSource(serviceStopToken, prereqsOtherFaultOrCancellation.Token))
            {
                CancellableAsyncFunc<MalTrainingData> trainingDataAsyncFunc = new CancellableAsyncFunc<MalTrainingData>(
                       () => LoadTrainingDataOnInitAsync(initialTrainingDataLoader, trainingDataCancel.Token), trainingDataOtherFaultOrCancellation);

                CancellableTask<MalTrainingData> trainingDataTask = trainingDataAsyncFunc.StartTaskEnsureExceptionsWrapped();

                CancellableAsyncFunc<IDictionary<int, IList<int>>> prereqsAsyncFunc = new CancellableAsyncFunc<IDictionary<int, IList<int>>>(
                    () => LoadPrereqsOnInit(initialTrainingDataLoader, prereqsCancel.Token), prereqsOtherFaultOrCancellation);

                CancellableTask<IDictionary<int, IList<int>>> prereqsTask = prereqsAsyncFunc.StartTaskEnsureExceptionsWrapped();

                AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(trainingDataTask, prereqsTask).ConfigureAwait(false).GetAwaiter().GetResult();

                return (trainingDataTask.Task.Result, prereqsTask.Task.Result);
            }
        }

        private static async Task<MalTrainingData> LoadTrainingDataOnInitAsync(IMalTrainingDataLoader trainingDataLoader, CancellationToken cancellationToken)
        {
            Logging.Log.Info("Loading training data.");
            Stopwatch timer = Stopwatch.StartNew();
            MalTrainingData trainingData;
            try
            {
                trainingData = await trainingDataLoader.LoadMalTrainingDataAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Logging.Log.Info("Canceled loading training data.");
                throw;
            }
            GC.Collect();
            timer.Stop();

            Logging.Log.InfoFormat("Training data loaded. {0} users, {1} animes, {2} entries. Took {3}.",
                trainingData.Users.Count, trainingData.Animes.Count,
                trainingData.Users.Keys.Sum(userId => trainingData.Users[userId].Entries.Count),
                timer.Elapsed);
            Logging.Log.InfoFormat("Memory use: {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            return trainingData;
        }

        private static async Task<IDictionary<int, IList<int>>> LoadPrereqsOnInit(IMalTrainingDataLoader trainingDataLoader, CancellationToken cancellationToken)
        {
            Logging.Log.Info("Loading prerequisites.");
            Stopwatch timer = Stopwatch.StartNew();
            IDictionary<int, IList<int>> prereqs;
            try
            {
                prereqs = await trainingDataLoader.LoadPrerequisitesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Logging.Log.Info("Canceled loading prerequisites.");
                throw;
            }
            timer.Stop();

            int numPrereqs = prereqs.Values.Sum(prereqList => prereqList.Count);
            Logging.Log.InfoFormat("Prerequisites loaded. {0} prerequisites for {1} animes. Took {2}.",
                numPrereqs, prereqs.Count, timer.Elapsed);
            Logging.Log.InfoFormat("Memory use {0} bytes", GC.GetTotalMemory(forceFullCollection: false));

            return prereqs;
        }

        // Loads all configured rec sources into the rec service in parallel.
        // Does not return until complete or serviceStopToken is signaled.
        private static void LoadRecSources(TcpRecService recService, Config config, CancellationToken serviceStopToken)
        {
            if (config.RecSources.Count == 0)
            {
                Logging.Log.Info("No rec sources configured.");
                return;
            }

            Logging.Log.InfoFormat("Loading {0} rec sources.", config.RecSources.Count);

            List<ICancellableTask> recSourceLoadTasks = new List<ICancellableTask>(config.RecSources.Count);

            using (CancellationTokenSource anyTaskFaultedOrCanceled = new CancellationTokenSource())
            using (CancellationTokenSource cancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(serviceStopToken, anyTaskFaultedOrCanceled.Token))
            {
                foreach (DTO.LoadRecSourceRequest recSourceConfigX in config.RecSources)
                {
                    DTO.LoadRecSourceRequest recSourceConfig = recSourceConfigX; // Don't capture the loop variable
                    Task loadRecSourceTask = Task.Factory.StartNew(() =>
                    {
                        recService.LoadRecSource(recSourceConfig, cancelTokenSource.Token);
                    }, cancelTokenSource.Token);
                    recSourceLoadTasks.Add(new CancellableTask(loadRecSourceTask, anyTaskFaultedOrCanceled));
                }

                try
                {
                    AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(recSourceLoadTasks).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                    Logging.Log.Info("Canceled loading rec sources.");
                    throw;
                }
            }

            recService.FinalizeRecSources(serviceStopToken);
        }
    }
}
