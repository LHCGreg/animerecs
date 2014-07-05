using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.DAL;

namespace AnimeRecs.RecService
{
    internal class TcpRecService : IDisposable
    {
        private TcpListener Listener { get; set; }
        private Thread ListenerThread { get; set; }
        private bool m_stop = false;
        private object m_syncHandle = new object();

        private Dictionary<int, Task> m_runningTasks = new Dictionary<int, Task>();

        private RecServiceState m_state;

        public TcpRecService(IMalTrainingDataLoaderFactory trainingDataLoaderFactory, int portNumber)
        {
            m_state = new RecServiceState(trainingDataLoaderFactory);

            try
            {
                Listener = new TcpListener(new IPEndPoint(IPAddress.Any, portNumber));
            }
            catch
            {
                m_state.Dispose();
                throw;
            }
        }

        public void Start()
        {
            const int maxPendingConnections = 100;
            Listener.Start(maxPendingConnections);
            Logging.Log.Debug("Started listening.");
            ListenerThread = new Thread(ListenerEntryPoint);
            ListenerThread.IsBackground = true;
            ListenerThread.Name = "Listener";
            ListenerThread.Start();
            Logging.Log.Debug("Listener thread started.");
        }

        private void ListenerEntryPoint()
        {
            Logging.Log.Debug("Listener thread entry point.");
            while (true)
            {
                lock (m_syncHandle)
                {
                    if (m_stop)
                    {
                        break;
                    }
                }

                try
                {
                    Logging.Log.Debug("Ready to accept another client.");
                    TcpClient client = Listener.AcceptTcpClient();
                    Logging.Log.DebugFormat("Accepted client {0}", client.Client.RemoteEndPoint.ToString());
                    Task connectionHandlerTask = new Task(ConnectionEntryPoint, client);
                    lock (m_syncHandle)
                    {
                        m_runningTasks[connectionHandlerTask.Id] = connectionHandlerTask;
                    }
                    connectionHandlerTask.Start();
                }
                catch (Exception ex)
                {
                    if (ex is SocketException && ((SocketException)ex).SocketErrorCode == SocketError.Interrupted)
                    {
                        Logging.Log.Debug("Listen was interrupted.");
                    }
                    else
                    {
                        Logging.Log.ErrorFormat("Error accepting a client: {0}", ex, ex.Message);
                    }
                }
            }
        }

        private void ConnectionEntryPoint(object clientObj)
        {
            Logging.Log.Debug("Servicer thread entry.");
            using (TcpClient client = (TcpClient)clientObj)
            {
                try
                {  
                    const int readTimeout = 3000;
                    const int writeTimeout = 3000;
                    client.ReceiveTimeout = readTimeout;
                    client.SendTimeout = writeTimeout;
                    ConnectionServicer servicer = new ConnectionServicer(client, m_state);
                    servicer.ServiceConnection();
                }
                catch (Exception ex)
                {
                    Logging.Log.ErrorFormat("Error servicing connection: {0}", ex, ex.Message);
                }
            }

            lock(m_syncHandle)
            {
                m_runningTasks.Remove(Task.CurrentId.Value);
            }
        }

        public void Dispose()
        {
            try
            {
                Logging.Log.Debug("Telling listener thread to stop.");
                // Tells the listener thread to stop before trying to do another Accept
                lock (m_syncHandle)
                {
                    m_stop = true;
                }
                // Stops the listener thread from its Accept if its waiting for a connection like it is most of the time.
                if (Listener != null)
                    Listener.Stop();

                Logging.Log.Debug("Waiting for listener thread to finish.");
                // Wait for listener thread to finish.
                if (ListenerThread != null)
                    ListenerThread.Join();

                // Wait for pending operations to complete
                Task[] runningTasks;
                lock (m_syncHandle)
                {
                    runningTasks = m_runningTasks.Values.ToArray();
                }

                Logging.Log.InfoFormat("Waiting for {0} operations to complete.", runningTasks.Length);

                Task.WaitAll(runningTasks);

                // Dispose of service state
                m_state.Dispose();
            }
            catch (Exception ex)
            {
                // Hopefully this is never reached.
                Logging.Log.ErrorFormat("Error while cleaning up: {0}", ex, ex.Message);
            }
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