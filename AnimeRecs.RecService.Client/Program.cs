using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using MiscUtil.IO;
using Newtonsoft.Json;
using MalApi;
using AnimeRecs.DAL;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.Client.Registrations.Output;

namespace AnimeRecs.RecService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);

            if (commandLine.ShowHelp)
            {
                commandLine.DisplayHelp(Console.Out);
                return;
            }

            using (AnimeRecsClient client = new AnimeRecsClient(commandLine.PortNumber))
            {
                if (commandLine.Operation.Equals("raw", StringComparison.OrdinalIgnoreCase))
                {
                    using (TcpClient rawClient = new TcpClient("localhost", commandLine.PortNumber))
                    {
                        byte[] jsonBytes = Encoding.UTF8.GetBytes(commandLine.RawJson);
                        rawClient.Client.Send(jsonBytes);

                        using (NetworkStream socketStream = rawClient.GetStream())
                        {
                            rawClient.Client.Shutdown(SocketShutdown.Send);
                            byte[] responseJsonBytes = StreamUtil.ReadFully(socketStream);
                            string responseJsonString = Encoding.UTF8.GetString(responseJsonBytes);
                            dynamic responseJson = JsonConvert.DeserializeObject<dynamic>(responseJsonString);
                            string prettyResponse = JsonConvert.SerializeObject(responseJson, Formatting.Indented);
                            Console.WriteLine(prettyResponse);
                        }
                    }
                }
                else if (commandLine.Operation.Equals(OpNames.Ping, StringComparison.OrdinalIgnoreCase))
                {
                    string pingResponse = client.Ping(commandLine.PingMessage);
                    Console.WriteLine("The service replied: {0}", pingResponse);
                }
                else if (commandLine.Operation.Equals(OpNames.ReloadTrainingData, StringComparison.OrdinalIgnoreCase))
                {
                    client.ReloadTrainingData(commandLine.ReloadMode);
                    Console.WriteLine("Training data reloaded.");
                }
                else if (commandLine.Operation.Equals(OpNames.LoadRecSource, StringComparison.OrdinalIgnoreCase))
                {
                    if (commandLine.RecSourceType.Equals(RecSourceTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
                    {
                        client.LoadRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AverageScoreRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                minUsersToCountAnime: commandLine.MinUsersToCountAnime,
                                useDropped: commandLine.UseDropped
                            )
                        );
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
                    {
                        client.LoadRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new MostPopularRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                useDropped: commandLine.UseDropped
                            )
                        );
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
                    {
                        client.LoadRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AnimeRecsRecSourceParams(
                                numRecommendersToUse: commandLine.NumRecommendersToUse,
                                fractionConsideredRecommended: commandLine.FractionRecommended,
                                minEpisodesToClassifyIncomplete: commandLine.MinEpisodesToCountIncomplete
                            )
                        );
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.BiasedMatrixFactorization, StringComparison.OrdinalIgnoreCase))
                    {
                        client.LoadRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource, commandLine.BiasedMatrixFactorizationParams);
                    }
                    else
                    {
                        throw new Exception("Oops! Missed a rec source type!");
                    }

                    Console.WriteLine("Load complete.");
                }
                else if (commandLine.Operation.Equals(OpNames.UnloadRecSource, StringComparison.OrdinalIgnoreCase))
                {
                    client.UnloadRecSource(commandLine.RecSourceName);
                    Console.WriteLine("Unload complete.");
                }
                else if (commandLine.Operation.Equals(OpNames.GetRecSourceType, StringComparison.OrdinalIgnoreCase))
                {
                    string recSourceType = client.GetRecSourceType(commandLine.RecSourceName);
                    Console.WriteLine("Type of rec source {0} is {1}.", commandLine.RecSourceName, recSourceType);
                }
                else if (commandLine.Operation.Equals(OpNames.GetMalRecs, StringComparison.OrdinalIgnoreCase))
                {
                    MalUserLookupResults lookup;

                    using (IMyAnimeListApi malApi = GetMalApi())
                    {
                        lookup = malApi.GetAnimeListForUser(commandLine.MalUsername);
                    }

                    Dictionary<int, RecEngine.MAL.MalListEntry> animeListEntries = new Dictionary<int, RecEngine.MAL.MalListEntry>();
                    foreach (MyAnimeListEntry entry in lookup.AnimeList)
                    {
                        animeListEntries[entry.AnimeInfo.AnimeId] = new RecEngine.MAL.MalListEntry((byte?)entry.Score, entry.Status, (short)entry.NumEpisodesWatched);
                    }

                    MalRecResults<IEnumerable<RecEngine.IRecommendation>> recs = client.GetMalRecommendations(
                        animeList: animeListEntries,
                        recSourceName: commandLine.RecSourceName,
                        numRecsDesired: commandLine.NumRecs,
                        targetScore: commandLine.TargetScore);

                    PrintRecs(recs, animeListEntries, commandLine.TargetScore);
                }
                else
                {
                    throw new Exception(string.Format("Oops, missed an operation: {0}", commandLine.Operation));
                }
            }
        }

        private static IMyAnimeListApi GetMalApi()
        {
            if (Config.UseDbAsMalApi)
            {
                return new PgMyAnimeListApi(Config.PgConnectionString);
            }
            else
            {
                return new MyAnimeListApi();
            }
        }

        private static void PrintRecs(MalRecResults<IEnumerable<RecEngine.IRecommendation>> recs, IDictionary<int, RecEngine.MAL.MalListEntry> animeList, decimal targetScore)
        {
            if (!recs.Results.Any())
            {
                Console.WriteLine("No recommendations.");
                return;
            }

            ResultsPrinter resultsPrinter = new ResultsPrinter();
            resultsPrinter.PrintResults(recs, animeList, targetScore);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.Client.
//
// AnimeRecs.RecService.Client is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.Client is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.Client.  If not, see <http://www.gnu.org/licenses/>.