using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AnimeRecs.DAL;
using AnimeRecs.RecService.Configuration;
using AnimeRecs.RecService.ClientLib;
#if MONO
using Mono.Unix;
using Mono.Unix.Native;
#endif

namespace AnimeRecs.RecService
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            System.Threading.Thread.CurrentThread.Name = "Main";
            Logging.SetUpLogging();

            try
            {
                CommandLineArgs commandLine = new CommandLineArgs(args);
                if (commandLine.ShowHelp)
                {
                    commandLine.DisplayHelp(Console.Out);
                    return;
                }

                ConfigRoot config = null;
                if (commandLine.ConfigFile != null)
                {
                    config = ConfigRoot.LoadFromFile(commandLine.ConfigFile);
                }

                string connectionString = ConfigurationManager.ConnectionStrings["Postgres"].ToString();
                PgMalTrainingDataLoaderFactory trainingDataLoaderFactory = new PgMalTrainingDataLoaderFactory(connectionString);
                using (TcpRecService recService = new TcpRecService(trainingDataLoaderFactory, commandLine.PortNumber))
                {
                    recService.Start();

                    if (config != null)
                    {
                        using (AnimeRecsClient client = new AnimeRecsClient(commandLine.PortNumber))
                        {
                            foreach (DTO.LoadRecSourceRequest recSourceToLoad in config.RecSources)
                            {
                                client.LoadRecSource(recSourceToLoad);
                            }

                            if (config.FinalizeAfterLoad)
                            {
                                client.FinalizeRecSources();
                            }
                        }
                    }

#if MONO
                    Logging.Log.InfoFormat("Started listening on port {0}. Press ctrl+c to stop.", commandLine.PortNumber);
                    WaitForUnixStopSignal();
#else

                    Logging.Log.InfoFormat("Started listening on port {0}. Press any key to stop.", commandLine.PortNumber);
                    Console.ReadKey();
#endif

                    Logging.Log.InfoFormat("Got stop signal.");
                }
                Logging.Log.InfoFormat("Shutdown complete.");
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Fatal error: {0}", ex, ex.Message);
                Environment.ExitCode = 1;
            }
        }

#if MONO
        static void WaitForUnixStopSignal()
        {
            UnixSignal[] signals = new UnixSignal[]
                        {
                            new UnixSignal(Signum.SIGINT),
                            new UnixSignal(Signum.SIGTERM)
                        };
            UnixSignal.WaitAny(signals);
        }
#endif
    }
}

// Copyright (C) 2014 Greg Najda
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