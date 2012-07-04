using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MiscUtil.IO;
using Newtonsoft.Json;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecService.DTO;
using AnimeRecs.MalApi;
using AnimeRecs.DAL;
using System.Globalization;

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
                    client.ReloadTrainingData();
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
                        animeListEntries[entry.AnimeInfo.AnimeId] = new RecEngine.MAL.MalListEntry(entry.Score, entry.Status, entry.NumEpisodesWatched);
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
            if (recs.Results is RecEngine.MAL.MalAnimeRecsResults)
            {
                MalRecResults<RecEngine.MAL.MalAnimeRecsResults> animeRecsResults = new MalRecResults<RecEngine.MAL.MalAnimeRecsResults>(
                    (RecEngine.MAL.MalAnimeRecsResults)recs.Results, recs.AnimeInfo);
                PrintAnimeRecsResults(animeRecsResults, animeList, targetScore);
                return;
            }

            if (!recs.Results.Any())
            {
                Console.WriteLine("No recommendations.");
                return;
            }

            int recNumber = 1;
            foreach (RecEngine.IRecommendation generalRec in recs.Results)
            {
                if (generalRec is RecEngine.AverageScoreRecommendation)
                {
                    if (recNumber == 1)
                    {
                        Console.WriteLine("     {0,-52} {1,-6} {2}", "Anime", "Avg", "# ratings");
                    }

                    RecEngine.AverageScoreRecommendation rec = (RecEngine.AverageScoreRecommendation)generalRec;
                    Console.WriteLine("{0,3}. {1,-52} {2,-6:f2} {3}", recNumber, recs.AnimeInfo[rec.ItemId].Title, rec.AverageScore, rec.NumRatings);
                }
                else if (generalRec is RecEngine.MostPopularRecommendation)
                {
                    if (recNumber == 1)
                    {
                        Console.WriteLine("     {0,-52} {1,4} {2}", "Anime", "Rank", "# ratings");
                    }

                    RecEngine.MostPopularRecommendation rec = (RecEngine.MostPopularRecommendation)generalRec;
                    Console.WriteLine("{0,3}. {1,-52} {2,4} {3}", recNumber, recs.AnimeInfo[rec.ItemId].Title, rec.PopularityRank, rec.NumRatings);

                }
                else if (generalRec is RecEngine.RatingPredictionRecommendation)
                {
                    if (recNumber == 1)
                    {
                        Console.WriteLine("     {0,-60} {1}", "Anime", "Prediction");
                    }

                    RecEngine.RatingPredictionRecommendation rec = (RecEngine.RatingPredictionRecommendation)generalRec;
                    Console.WriteLine("{0,3}. {1,-60} {2:F3}", recNumber, recs.AnimeInfo[rec.ItemId].Title, rec.PredictedRating);
                }
                else
                {
                    if (recNumber == 1)
                    {
                        Console.WriteLine("     {0,-65}", "Anime");
                    }

                    Console.WriteLine("{0,3}. {1.-65}", recNumber, recs.AnimeInfo[generalRec.ItemId].Title);
                }

                recNumber++;
            }
        }

        private static void PrintAnimeRecsResults(MalRecResults<RecEngine.MAL.MalAnimeRecsResults> results, IDictionary<int, RecEngine.MAL.MalListEntry> animeList, decimal targetScore)
        {
            int numRecommendersPrinted = 0;
            
            foreach (RecEngine.MAL.MalAnimeRecsRecommenderUser recommender in results.Results.Recommenders)
            {
                if (numRecommendersPrinted > 10)
                {
                    break;
                }
                
                string recsLikedString;

                if (recommender.NumRecsInCommon > 0)
                {
                    recsLikedString = string.Format("{0:P2}", (double) recommender.RecsLiked.Count / recommender.NumRecsInCommon);
                }
                else
                {
                    recsLikedString = string.Format("{0:P2}", 0);
                }

                Console.WriteLine("{0}'s recommendations ({1}/{2} {3} recs liked, {4:P2} - {5:P2} estimated compatibility",
                    recommender.Username, recommender.RecsLiked.Count, recommender.NumRecsInCommon, recsLikedString,
                    recommender.CompatibilityLowEndpoint ?? 0, recommender.CompatibilityHighEndpoint ?? 0);

                Console.WriteLine("{0,-52} {1,-5} {2,-4} {3,-5} {4,-4} {5}", "Anime", "State", "Like", "Their", "Your", "Avg");

                foreach (RecEngine.MAL.MalAnimeRecsRecommenderRecommendation recommendation in recommender.AllRecommendations.OrderBy(
                    rec => !recommender.RecsLiked.Contains(rec) && !recommender.RecsNotLiked.Contains(rec) ? 0 :
                        recommender.RecsLiked.Contains(rec) ? 1 :
                        2
                    )
                    .ThenByDescending(rec => rec.RecommenderScore)
                    .ThenByDescending(rec => rec.AverageScore))
                {
                    string status;
                    decimal? yourRating = null;
                    if (!animeList.ContainsKey(recommendation.MalAnimeId))
                    {
                        status = "-"; // not watched, not in list
                    }
                    else
                    {
                        if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Completed)
                            status = "comp";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Dropped)
                            status = "drop";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.OnHold)
                            status = "hold";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.PlanToWatch)
                            status = "plan";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Watching)
                            status = "watch";
                        else
                            status = "?";

                        yourRating = animeList[recommendation.MalAnimeId].Rating;
                    }

                    string yourRatingString;
                    if (yourRating != null)
                    {
                        yourRatingString = yourRating.Value.ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        yourRatingString = "-";
                    }

                    string likedString;
                    if (recommender.RecsLiked.Contains(recommendation))
                        likedString = "+";
                    else if (recommender.RecsNotLiked.Contains(recommendation))
                        likedString = "-";
                    else
                        likedString = "?";

                    Console.WriteLine("{0,-52} {1,-5} {2,-4} {3,-5:F0} {4,-4:F0} {5:F2}",
                        results.AnimeInfo[recommendation.MalAnimeId].Title, status, likedString, recommendation.RecommenderScore,
                        yourRatingString, recommendation.AverageScore);
                }

                Console.WriteLine();
                Console.WriteLine();

                numRecommendersPrinted++;
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