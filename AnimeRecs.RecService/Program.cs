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

        static int Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Logging.SetUpLogging();

            try
            {
                CommandLineArgs commandLine = new CommandLineArgs(args);
                if (commandLine.ShowHelp)
                {
                    commandLine.DisplayHelp(Console.Out);
                    return (int)ExitCode.Success;
                }

                StartServiceAndWait(commandLine);
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
        private static void StartServiceAndWait(CommandLineArgs commandLine)
        {
            // When a request to stop the service is sent, cancel this cancellation token source.
            using (CancellationTokenSource serviceStopper = new CancellationTokenSource())
            {
                RegisterCancellationHandlers(serviceStopper);

                try
                {
                    StartServiceAndWaitThrowOpCanceled(commandLine, serviceStopper.Token);
                }
                catch (OperationCanceledException)
                {
                    Logging.Log.Info("Rec service shutdown complete.");
                }
            }
        }

        static object SigHandlersLock = new object();

        static ConsoleCancelEventHandler SigIntHandler = null;

#if NETCORE
        static Action<AssemblyLoadContext> SigTermHandler = null;
#endif

        static void RegisterCancellationHandlers(CancellationTokenSource serviceStopper)
        {
            // Set up how the service will be stopped.
            // Console.CancelKeyPress corresponds to pressing ctrl+c in the console on Windows
            // and to SIGINT on Linux, which can be sent in a console with ctrl+c or programtically or from a shell with kill.
            // assemblyLoadContext.Unloading corresponds to SIGTERM on Linux and nothing(?) on Windows.
            // AssemblyLoadContext is only on .net core, not .net 4.7.

            lock (SigHandlersLock)
            {
                // Don't need to wait for shutdown inside the event handler for SIGINT because the process doesn't die after the event handler returns.
                // The SIGTERM handler on the other hand...not sure but the process might die after the event handler returns.
                SigIntHandler = (sender, eventArgs) => { StopService(serviceStopper, waitForShutdown: false); eventArgs.Cancel = true; };
                Console.CancelKeyPress += SigIntHandler;
#if NETCORE
                var assemblyLoadContext = AssemblyLoadContext.GetLoadContext(typeof(Program).GetTypeInfo().Assembly);

                // Sleep for a bit after everything is shut down to give logs a chance to flush
                SigTermHandler = context => { StopService(serviceStopper, waitForShutdown: true); Thread.Sleep(TimeSpan.FromMilliseconds(500)); };
                assemblyLoadContext.Unloading += SigTermHandler;
#endif
            }
        }

        static void UnregisterCancellationHandlers()
        {
            lock (SigHandlersLock)
            {
                if (SigIntHandler != null)
                {
                    Console.CancelKeyPress -= SigIntHandler;
                    SigIntHandler = null;
#if NETCORE
                    var assemblyLoadContext = AssemblyLoadContext.GetLoadContext(typeof(Program).GetTypeInfo().Assembly);
                    assemblyLoadContext.Unloading -= SigTermHandler;
                    SigTermHandler = null;
#endif
                }
            }
        }

        static void StopService(CancellationTokenSource serviceStopper, bool waitForShutdown)
        {
            lock (SigHandlersLock)
            {
                // If handlers are already unregistered, serviceStopper has been disposed so we shouldn't access it.
                // This might happen if signals are sent rapidly.
                if (SigIntHandler == null)
                {
                    return;
                }

                // If we're not already in the process of stopping, stop by setting cancellation on the service stopper token.
                if (!serviceStopper.IsCancellationRequested)
                {
                    Logging.Log.Info("Stopping rec service...");
                    serviceStopper.Cancel();
                }

                // Unregister handlers so we stop getting shutdown signals.
                // There could still be signals sent while we were inside lock(SigHandlersLock), waiting to enter the lock.
                UnregisterCancellationHandlers();
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
        private static void StartServiceAndWaitThrowOpCanceled(CommandLineArgs commandLine, CancellationToken serviceStopToken)
        {
            serviceStopToken.ThrowIfCancellationRequested();
            Config config = Config.LoadFromFile(commandLine.ConfigFile);
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

// Copyright (C) 2017 Greg Najda
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