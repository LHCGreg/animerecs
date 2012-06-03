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
            MalTrainingData trainingData;
            using (IMalTrainingDataLoader trainingDataLoader = trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                trainingData = trainingDataLoader.LoadMalTrainingData();
            }
            m_state = new RecServiceState(trainingData);
            Listener = new TcpListener(new IPEndPoint(IPAddress.Any, portNumber));
        }

        public void Start()
        {
            Listener.Start(100);
            ListenerThread = new Thread(ListenerEntryPoint);
            ListenerThread.IsBackground = true;
            ListenerThread.Name = "Rec Service Listener Thread";
            ListenerThread.Start();
        }

        private void ListenerEntryPoint()
        {
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
                    TcpClient client = Listener.AcceptTcpClient();
                    //Task newTask = Task.Factory.StartNew(ConnectionEntryPoint, client);
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
                        ; // We're in the process of shutting down, don't log an error.
                    }
                    else
                    {
                        Console.WriteLine(ex); // TODO: Log error
                    }
                }
            }
        }

        private void ConnectionEntryPoint(object clientObj)
        {
            using (TcpClient client = (TcpClient)clientObj)
            {
                try
                {
                    using (NetworkStream clientStream = client.GetStream())
                    {
                        const int readTimeout = 3000;
                        const int writeTimeout = 3000;
                        clientStream.ReadTimeout = readTimeout;
                        clientStream.WriteTimeout = writeTimeout;
                        ConnectionServicer servicer = new ConnectionServicer(clientStream, m_state);
                        servicer.ServiceConnection();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex); // TODO: Log error
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
                // Tells the listener thread to stop before trying to do another Accept
                lock (m_syncHandle)
                {
                    m_stop = true;
                }
                // Stops the listener thread from its Accept if its waiting for a connection like it is most of the time.
                if (Listener != null)
                    Listener.Stop();
                // Wait for listener thread to finish.
                if (ListenerThread != null)
                    ListenerThread.Join();

                // Wait for pending operations to complete
                Task[] runningTasks;
                lock (m_syncHandle)
                {
                    runningTasks = m_runningTasks.Values.ToArray();
                }

                Task.WaitAll(runningTasks);

                // Dispose of service state
                m_state.Dispose();
            }
            catch (Exception ex)
            {
                // Hopefully this is never reached.
                Console.WriteLine(ex); // TODO: Log error
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