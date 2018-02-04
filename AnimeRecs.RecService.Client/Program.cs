using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using MalApi;
using AnimeRecs.DAL;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.Client.Registrations.Output;
using AnimeRecs.Utils;
using System.Threading;
using System.Net;
using System.Diagnostics;

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

            Config config = Config.LoadFromFile(commandLine.ConfigFile);

            using (AnimeRecsClient client = new AnimeRecsClient(commandLine.PortNumber))
            {
                if (commandLine.Operation.Equals("raw", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] requestJsonBytes = Encoding.UTF8.GetBytes(commandLine.RawJson);
                    int requestLength = requestJsonBytes.Length;
                    int requestLengthNetworkOrder = IPAddress.HostToNetworkOrder(requestLength);
                    byte[] requestLengthBytes = BitConverter.GetBytes(requestLengthNetworkOrder);

                    byte[] requestBytes = new byte[requestLengthBytes.Length + requestJsonBytes.Length];
                    requestLengthBytes.CopyTo(requestBytes, index: 0);
                    requestJsonBytes.CopyTo(requestBytes, index: requestLengthBytes.Length);

                    using (Socket rawClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        TimeSpan sendTimeout = TimeSpan.FromSeconds(5);
                        rawClientSocket.SendAllAsync(requestBytes, sendTimeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                        rawClientSocket.Shutdown(SocketShutdown.Send);

                        byte[] responseLengthBuffer = rawClientSocket.ReceiveAllAsync(numBytesToReceive: 4, receiveAllTimeout: TimeSpan.FromMinutes(3), cancellationToken: CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                        int responseLengthNetworkOrder = BitConverter.ToInt32(responseLengthBuffer, 0);
                        int responseLength = IPAddress.NetworkToHostOrder(responseLengthNetworkOrder);
                        byte[] responseJsonBytes = rawClientSocket.ReceiveAllAsync(responseLength, receiveAllTimeout: TimeSpan.FromSeconds(5), cancellationToken: CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                        string responseJsonString = Encoding.UTF8.GetString(responseJsonBytes);
                        dynamic responseJson = JsonConvert.DeserializeObject<dynamic>(responseJsonString);
                        string prettyResponse = JsonConvert.SerializeObject(responseJson, Formatting.Indented);
                        Console.WriteLine(prettyResponse);
                    }
                }
                else if (commandLine.Operation.Equals(OperationTypes.Ping, StringComparison.OrdinalIgnoreCase))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(3);
                    Stopwatch timer = Stopwatch.StartNew();
                    string pingResponse = client.PingAsync(commandLine.PingMessage, timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    timer.Stop();
                    Console.WriteLine($"The service replied: {pingResponse} (took {timer.Elapsed})");
                }
                else if (commandLine.Operation.Equals(OperationTypes.ReloadTrainingData, StringComparison.OrdinalIgnoreCase))
                {
                    TimeSpan timeout = TimeSpan.FromMinutes(3);
                    client.ReloadTrainingDataAsync(commandLine.ReloadMode, commandLine.Finalize, timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    Console.WriteLine("Training data reloaded.");
                }
                else if (commandLine.Operation.Equals(OperationTypes.FinalizeRecSources, StringComparison.OrdinalIgnoreCase))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(5);
                    client.FinalizeRecSourcesAsync(timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                    Console.WriteLine("Rec sources finalized.");
                }
                else if (commandLine.Operation.Equals(OperationTypes.LoadRecSource, StringComparison.OrdinalIgnoreCase))
                {
                    if (commandLine.RecSourceType.Equals(RecSourceTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
                    {
                        TimeSpan timeout = TimeSpan.FromSeconds(30);
                        client.LoadRecSourceAsync(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AverageScoreRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                minUsersToCountAnime: commandLine.MinUsersToCountAnime,
                                useDropped: commandLine.UseDropped
                            ),
                            timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
                    {
                        TimeSpan timeout = TimeSpan.FromSeconds(30);
                        client.LoadRecSourceAsync(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new MostPopularRecSourceParams(
                                minEpisodesToCountIncomplete: commandLine.MinEpisodesToCountIncomplete,
                                useDropped: commandLine.UseDropped
                            ), timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
                    {
                        TimeSpan timeout = TimeSpan.FromSeconds(60);
                        client.LoadRecSourceAsync(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            new AnimeRecsRecSourceParams(
                                numRecommendersToUse: commandLine.NumRecommendersToUse,
                                fractionConsideredRecommended: commandLine.FractionRecommended,
                                minEpisodesToClassifyIncomplete: commandLine.MinEpisodesToCountIncomplete
                            ), timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else if (commandLine.RecSourceType.Equals(RecSourceTypes.BiasedMatrixFactorization, StringComparison.OrdinalIgnoreCase))
                    {
                        TimeSpan timeout = TimeSpan.FromMinutes(3);
                        client.LoadRecSourceAsync(commandLine.RecSourceName, commandLine.ReplaceExistingRecSource,
                            commandLine.BiasedMatrixFactorizationParams, timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                    {
                        throw new Exception("Oops! Missed a rec source type!");
                    }

                    Console.WriteLine("Load complete.");
                }
                else if (commandLine.Operation.Equals(OperationTypes.UnloadRecSource, StringComparison.OrdinalIgnoreCase))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(10);
                    client.UnloadRecSourceAsync(commandLine.RecSourceName, timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    Console.WriteLine("Unload complete.");
                }
                else if (commandLine.Operation.Equals(OperationTypes.GetRecSourceType, StringComparison.OrdinalIgnoreCase))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(3);
                    string recSourceType = client.GetRecSourceTypeAsync(commandLine.RecSourceName, timeout, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    Console.WriteLine("Type of rec source {0} is {1}.", commandLine.RecSourceName, recSourceType);
                }
                else if (commandLine.Operation.Equals(OperationTypes.GetMalRecs, StringComparison.OrdinalIgnoreCase))
                {
                    MalUserLookupResults lookup;

                    using (IMyAnimeListApi malApi = GetMalApi(config))
                    {
                        lookup = malApi.GetAnimeListForUser(commandLine.MalUsername);
                    }

                    Dictionary<int, RecEngine.MAL.MalListEntry> animeListEntries = new Dictionary<int, RecEngine.MAL.MalListEntry>();
                    foreach (MyAnimeListEntry entry in lookup.AnimeList)
                    {
                        animeListEntries[entry.AnimeInfo.AnimeId] = new RecEngine.MAL.MalListEntry((byte?)entry.Score, entry.Status, (short)entry.NumEpisodesWatched);
                    }

                    TimeSpan timeout = TimeSpan.FromSeconds(10);
                    MalRecResults<IEnumerable<RecEngine.IRecommendation>> recs = client.GetMalRecommendationsAsync(
                        animeList: animeListEntries,
                        recSourceName: commandLine.RecSourceName,
                        numRecsDesired: commandLine.NumRecs,
                        targetScore: commandLine.TargetScore,
                        timeout: timeout,
                        cancellationToken: CancellationToken.None
                    )
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                    PrintRecs(recs, animeListEntries, commandLine.TargetScore);
                }
                else
                {
                    throw new Exception(string.Format("Oops, missed an operation: {0}", commandLine.Operation));
                }
            }
        }

        private static IMyAnimeListApi GetMalApi(Config config)
        {
            if (config.MalApiType == Config.ApiType.DB)
            {
                return new PgMyAnimeListApi(config.ConnectionStrings.AnimeRecs);
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
