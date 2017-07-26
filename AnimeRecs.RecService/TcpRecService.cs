using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using AnimeRecs.RecEngine.MAL;
using Nito.AsyncEx;
using AnimeRecs.Utils;

namespace AnimeRecs.RecService
{
    internal class TcpRecService : IDisposable
    {
        private int m_portNumber;
        private RecServiceState m_state;
        private CancellationTokenSource m_stopper;
        private Task m_listenerTask;

        // Keep track of in-flight connection servicer tasks so that when stopping the service we can wait
        // a bit for them to complete, and then cancel any that are still running after that.

        // Lock for m_connectionServicerTasks
        private object m_taskListLock = new object();

        // Interlocked.Increment will return 1 on the first call.
        private long m_nextConnectionServicerTaskId = 0;

        // Listener task adds to this dictionary. Connection servicer tasks remove themselves.
        // The dictionary key is an ID obtained by incrementing m_nextConnectionServicerTaskId
        // before starting the servicer task, and is passed into the servicer task so that it can
        // remove itself from this dictionary.
        private Dictionary<long, CancellableTask> m_connectionServicerTasks = new Dictionary<long, CancellableTask>();

        // Must not block
        public TcpRecService(int portNumber, IMalTrainingDataLoaderFactory trainingDataLoaderFactory, MalTrainingData trainingData, IDictionary<int, IList<int>> prereqs)
        {
            m_portNumber = portNumber;
            m_state = new RecServiceState(trainingDataLoaderFactory, trainingData, prereqs);
        }

        // Load rec source synchronously. Ideally rec source training would periodically check for cancellation
        // so that service can be stopped even if a rec source with massive startup time is loaded. But since
        // the service is stopping anyway, training takes place on a separate thread which is abandoned on cancellation.
        // For use on startup only, before calling Start().
        public void LoadRecSource(DTO.LoadRecSourceRequest recSourceConfig, CancellationToken cancellationToken)
        {
            if (m_state == null)
            {
                throw new ObjectDisposedException("TcpRecService");
            }

            if (m_listenerTask != null)
            {
                throw new InvalidOperationException("Can only load rec sources into TcpRecService before starting to listen on socket.");
            }

            try
            {
                // "Async" because of async locks
                m_state.LoadRecSourceAsync(recSourceConfig, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error loading rec source {0}: {1}", recSourceConfig.Name, ex.Message), ex);
            }
        }

        // For use on startup only, before calling Start().
        // Finalize the rec sources synchronously, freeing up memory used by training data.
        // The only blocking would be on locks, but the locks shouldn't be contended because the service
        // hasn't been started yet. The finalize operation requires a CancellationToken though, so we'll give it one.
        public void FinalizeRecSources(CancellationToken cancellationToken)
        {
            if (m_state == null)
            {
                throw new ObjectDisposedException("TcpRecService");
            }

            if (m_listenerTask != null)
            {
                throw new InvalidOperationException("Can only finalize rec sources via TcpRecService before starting to listen on socket.");
            }

            try
            {
                m_state.FinalizeRecSourcesAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error finalizing rec sources: {0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Does not block. Starts accepting and servicing connections on a background thread.
        /// </summary>
        public void Start()
        {
            if (m_state == null)
            {
                throw new ObjectDisposedException("TcpRecService");
            }

            if (m_listenerTask != null)
            {
                throw new InvalidOperationException("Can't start rec service when it's already started.");
            }

            try
            {
                const int maxPendingConnections = 100;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // TODO: Configurable listening address, IPv6
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, m_portNumber);
                socket.Bind(endpoint);
                socket.Listen(maxPendingConnections);
                Logging.Log.Debug("Started listening.");

                m_stopper = new CancellationTokenSource();
                m_listenerTask = Task.Factory.StartNew(() => ListenerEntryPoint(socket), m_stopper.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                Logging.Log.Debug("Listener thread started.");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error when starting to listen for connections: {0}", ex.Message), ex);
            }
        }

        private long GetNextConnectionServicerTaskId()
        {
            return Interlocked.Increment(ref m_nextConnectionServicerTaskId);
        }

        // Should not throw.
        // Responsible for logging its own exceptions.
        // Runs on its own "long running" thread (does not occupy space in the thread pool).
        // Is canceled via m_stopper and waited on in Stop().
        private void ListenerEntryPoint(Socket socket)
        {
            Logging.Log.Debug("Listener thread entry point.");
            try
            {
                while (true)
                {
                    Logging.Log.Debug("Ready to accept a client.");
                    Task<Socket> acceptTask = socket.AcceptAsync();

                    // Throws OperationCanceledException if cancellation was requested
                    Socket clientSocket;
                    try
                    {
                        clientSocket = acceptTask.WaitAsync(m_stopper.Token).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch (Exception ex) when (!(ex is OperationCanceledException))
                    {
                        Logging.Log.ErrorFormat("Error accepting a client: {0}", ex, ex.Message);
                        continue;
                    }

                    Logging.Log.DebugFormat("Accepted client {0}", clientSocket.RemoteEndPoint);

                    CancellationTokenSource connectionHandlerCanceler = new CancellationTokenSource();
                    long handlerTaskId = GetNextConnectionServicerTaskId();
                    lock (m_taskListLock)
                    {
                        Task connectionHandlerTask = Task.Run(async () => await NewConnectionEntryPointAsync(clientSocket, handlerTaskId, connectionHandlerCanceler.Token));

                        // Add task and cancellation token to a dict so service stop can wait a bit for it to complete and then cancel it.
                        // It's important that the lock on m_taskListLock starts before the task is started.
                        // Otherwise the task could potentially complete and attempt to remove itself
                        // from m_connectionServicerTasks before the following line adds it.
                        // Use an incrementing long that this class keeps track of instead of the task's ID because
                        // task IDs can behave unexpectedly when async is involved - best to just generate
                        // our own ID and pass it to the task. See https://blog.stephencleary.com/2013/03/taskcurrentid-in-async-methods.html
                        m_connectionServicerTasks[handlerTaskId] = new CancellableTask(connectionHandlerTask, connectionHandlerCanceler);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Logging.Log.Debug("Listener thread got a cancellation.");
                socket.Dispose();
                Logging.Log.Debug("Listener socket disposed.");
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Listener thread got an exception: {0}", ex, ex.Message);
                socket.Dispose();
                Logging.Log.Debug("Listener socket disposed.");
            }
        }

        private async Task NewConnectionEntryPointAsync(Socket clientSocket, long handlerTaskId, CancellationToken cancellationToken)
        {
            // Start connection servicer on a thread pool thread so the listener thread can get right back to accepting connections.
            // Run any blocking operations asynchronously so that the thread pool thread is yielded.
            // The listener thread isn't going to wait around for this task to complete, so this function
            // is responsible for logging its own errors and not letting exceptions escape, except OperationCanceledException if canceled.
            // This function is responsible for removing its task from m_connectionServicerTasks when complete.

            Logging.Log.Debug("Servicer task entry.");
            try
            {
                using (clientSocket)
                {
                    // TODO: Make these configurable
                    TimeSpan readTimeout = TimeSpan.FromSeconds(3);
                    TimeSpan writeTimeout = TimeSpan.FromSeconds(3);
                    ConnectionServicer servicer = new ConnectionServicer(clientSocket, m_state, readTimeout, writeTimeout, cancellationToken);
                    await servicer.ServiceConnectionAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                Logging.Log.ErrorFormat("Error servicing connection: {0}", ex, ex.Message);
            }
            finally
            {
                lock (m_taskListLock)
                {
                    m_connectionServicerTasks[handlerTaskId].CancellationTokenSource.Dispose();
                    m_connectionServicerTasks.Remove(handlerTaskId);
                }

                Logging.Log.Debug("Servicer task exit.");
            }
        }

        /// <summary>
        /// Stops listening for new connections and waits for running connection servicers to finish,
        /// up to the time given by connectionDrainTimeout. After that, a cancellation is issued
        /// to the connection servicers but they are not waited on.
        /// Ideally, potentially long-running operations would periodically check for cancellation.
        /// But since the program is shutting down anyway, it doesn't really matter.
        /// 
        /// Once the service is stopped, it cannot be started again.
        /// </summary>
        /// <param name="connectionDrainTimeout"></param>
        public void Stop(TimeSpan connectionDrainTimeout)
        {
            if (m_state == null)
            {
                return; // already stopped
            }

            // Only stop if the service got started.
            if (m_stopper != null)
            {
                StopService(connectionDrainTimeout);
            }

            // Make the memory available for garbage collection
            m_state = null;
        }

        private void StopService(TimeSpan connectionDrainTimeout)
        {
            // Stop listening for new connections and WAIT for the listener task to stop.
            // It SHOULD be minimally blocking.
            Logging.Log.Debug("Telling listener thread to stop.");
            m_stopper.Cancel();
            Logging.Log.Debug("Waiting for listener thread to finish.");
            try
            {
                m_listenerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logging.Log.ErrorFormat("Waiting for listener task to end threw an exception, this should not happen: {0}", ex, ex.Message);
            }
            m_stopper.Dispose();
            Logging.Log.Debug("Listener thread finished.");

            // Wait a bit for connections being serviced to finish.
            CancellableTask[] outstandingConnectionTasks;
            lock (m_taskListLock)
            {
                outstandingConnectionTasks = m_connectionServicerTasks.Values.ToArray();
            }

            if (outstandingConnectionTasks.Length > 0)
            {
                try
                {
                    Task[] tasks = outstandingConnectionTasks.Select(t => t.Task).ToArray();
                    Logging.Log.InfoFormat("Waiting a bit for {0} operations to complete.", outstandingConnectionTasks.Length);
                    Task.WaitAll(tasks, connectionDrainTimeout);
                }
                catch (AggregateException aggEx)
                {
                    AggregateException flattened = aggEx.Flatten();
                    foreach (Exception ex in flattened.InnerExceptions)
                    {
                        if (!(ex is OperationCanceledException))
                        {
                            Logging.Log.ErrorFormat("Connection servicer task threw an exception, this should not happen: {0}", ex, ex.Message);
                        }
                    }
                }
            }

            // Issue a cancellation but don't bother waiting for outstanding tasks to complete.
            lock (m_taskListLock)
            {
                outstandingConnectionTasks = m_connectionServicerTasks.Values.ToArray();
            }

            if (outstandingConnectionTasks.Length > 0)
            {
                Logging.Log.InfoFormat("Canceling {0} operations, not waiting for them.", outstandingConnectionTasks.Length);
                foreach (CancellableTask task in outstandingConnectionTasks)
                {
                    task.Cancel();
                }

                // Don't bother waiting for outstanding tasks to complete
                //try
                //{
                //    Task[] tasks = outstandingConnectionTasks.Select(t => t.Task).ToArray();
                //    Logging.Log.InfoFormat("Canceled {0} operations, waiting for them to complete or acknowledge cancellation.", outstandingConnectionTasks.Length);
                //    Task.WaitAll(tasks);
                //}
                //catch (AggregateException aggEx)
                //{
                //    AggregateException flattened = aggEx.Flatten();
                //    foreach (Exception ex in flattened.InnerExceptions)
                //    {
                //        if (!(ex is OperationCanceledException))
                //        {
                //            Logging.Log.ErrorFormat("Connection servicer task threw an exception, this should not happen: {0}", ex, ex.Message);
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// Stops the service and waits up to 5 seconds for existing connections to be serviced.
        /// </summary>
        public void Dispose()
        {
            Stop(TimeSpan.FromSeconds(5));
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