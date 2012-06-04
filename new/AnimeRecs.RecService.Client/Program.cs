using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MiscUtil.IO;
using Newtonsoft.Json;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecService.DTO;

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
                else if (commandLine.Operation.Equals(OpNames.Ping))
                {
                    string pingResponse = client.Ping(commandLine.PingMessage);
                    Console.WriteLine("The service replied: {0}", pingResponse);
                }
                else if (commandLine.Operation.Equals(OpNames.LoadRecSource))
                {
                    if (commandLine.RecSourceType.Equals(RecSourceTypes.AverageScore))
                    {
                        client.LoadAverageScoreRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AverageScoreRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                minUsersToCountAnime: commandLine.MinUsersToCountAnime,
                                useDropped: commandLine.UseDropped
                            )
                        );
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.MostPopular))
                    {
                        client.LoadMostPopularRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new MostPopularRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                useDropped: commandLine.UseDropped
                            )
                        );
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.AnimeRecs))
                    {
                        client.LoadAnimeRecsRecSource(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AnimeRecsRecSourceParams(
                                numRecommendersToUse: commandLine.NumRecommendersToUse,
                                fractionConsideredRecommended: commandLine.FractionRecommended,
                                minEpisodesToClassifyIncomplete: commandLine.MinEpisodesToCountIncomplete
                            )
                        );
                    }
                    Console.WriteLine("Load complete.");
                }
                else
                {
                    throw new Exception(string.Format("Oops, missed an operation: {0}", commandLine.Operation));
                }
            }
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