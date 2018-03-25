using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AnimeRecs.DAL;
using AnimeRecs.UpdateStreams.Crunchyroll;
using CsvHelper;
using System.Threading.Tasks;
using System.Threading;
using AnimeRecs.Utils;

namespace AnimeRecs.UpdateStreams
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);

            if (commandLine.Op == Operation.CreateCsv)
            {
                CreateCsv(commandLine);
            }
            else if (commandLine.Op == Operation.GenerateSql)
            {
                GenerateSql(commandLine.InputFile, commandLine.OutputFile);
            }
            else
            {
                throw new Exception("Oops, missed an operation.");
            }
        }

        /// <summary>
        /// If null inputfile, create fresh.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        static void CreateCsv(CommandLineArgs args)
        {
            string inputFile = args.InputFile;
            string outputFile = args.OutputFile;

            // Read in existing mapping from the input csv. If no input csv was specified, treat it as an empty csv.
            List<CsvRow> inputCsvRows;
            if (inputFile == null)
            {
                inputCsvRows = new List<CsvRow>();
            }
            else
            {
                inputCsvRows = LoadCsv(inputFile);
            }

            // Index streams available by the streaming service and the anime name used by the streaming service.
            Dictionary<StreamingService, Dictionary<string, List<CsvRow>>> rowsByServiceAndAnime = new Dictionary<StreamingService, Dictionary<string, List<CsvRow>>>();

            foreach (CsvRow csvRow in inputCsvRows)
            {
                if (!rowsByServiceAndAnime.ContainsKey(csvRow.Service))
                {
                    rowsByServiceAndAnime[csvRow.Service] = new Dictionary<string, List<CsvRow>>();
                }
                if (!rowsByServiceAndAnime[csvRow.Service].ContainsKey(csvRow.AnimeName))
                {
                    rowsByServiceAndAnime[csvRow.Service][csvRow.AnimeName] = new List<CsvRow>();
                }
                rowsByServiceAndAnime[csvRow.Service][csvRow.AnimeName].Add(csvRow);
            }

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();

            using (WebClient webClient = new WebClient())
            // Use Firefox driver with headless Firefox for Funimation to get around Incapsula.
            // Use Firefox and not Chrome because javascript cannot be disabled with headless Chrome at this time.
            // Javascript slows the process down and is not needed currently.
            using (FirefoxDriverWebClient funimationWebClient = new FirefoxDriverWebClient(args.GeckoDriverDirectory))
            {
                List<IAnimeStreamInfoSource> streamInfoSources = GetStreamInfoSources(args, webClient, funimationWebClient);
                using (CancellationTokenSource cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
                {
                    CancellableAsyncFunc<ICollection<AnimeStreamInfo>>[] streamFuncs = streamInfoSources.Select(source => new CancellableAsyncFunc<ICollection<AnimeStreamInfo>>(
                        () => source.GetAnimeStreamInfoAsync(cancellation.Token), cancellation)
                    ).ToArray();

                    CancellableTask<ICollection<AnimeStreamInfo>>[] streamTasks = AsyncUtils.StartTasksEnsureExceptionsWrapped(streamFuncs);
                    try
                    {
                        AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(streamTasks).GetAwaiter().GetResult();
                    }
                    catch (OperationCanceledException ex)
                    {
                        throw new Exception("Getting streams timed out.", ex);
                    }

                    foreach (CancellableTask<ICollection<AnimeStreamInfo>> streamTask in streamTasks)
                    {
                        streams.AddRange(streamTask.Task.Result);
                    }
                }
            }

            Dictionary<StreamingService, Dictionary<string, List<AnimeStreamInfo>>> streamsByServiceAndAnime = new Dictionary<StreamingService, Dictionary<string, List<AnimeStreamInfo>>>();

            foreach (AnimeStreamInfo stream in streams)
            {
                if (!streamsByServiceAndAnime.ContainsKey(stream.Service))
                {
                    streamsByServiceAndAnime[stream.Service] = new Dictionary<string, List<AnimeStreamInfo>>();
                }
                if (!streamsByServiceAndAnime[stream.Service].ContainsKey(stream.AnimeName))
                {
                    streamsByServiceAndAnime[stream.Service][stream.AnimeName] = new List<AnimeStreamInfo>();
                }
                streamsByServiceAndAnime[stream.Service][stream.AnimeName].Add(stream);
            }

            Console.WriteLine("Writing out csv.");

            // Write a new csv mapping to the output file. If MAL anime ids or n/a was present in the input file for a certain
            // streaming service/anime name/URL combination, use them. Otherwise, leave the MAL anime id column blank
            // for a human operator to fill in.
            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (StreamWriter output = new StreamWriter(outputStream, Encoding.UTF8))
            {
                string header = "Service,Anime,URL,MAL ID (or n/a)";
                output.Write(header); // Newline gets written by the first row's data

                foreach (AnimeStreamInfo streamInfo in streams
                    .OrderBy(stream => stream.Service.ToString())
                    .ThenBy(stream => stream.AnimeName)
                    .ThenBy(stream => stream.Url))
                {
                    List<CsvRow> existingCsvRows = new List<CsvRow>();
                    if (rowsByServiceAndAnime.ContainsKey(streamInfo.Service) && rowsByServiceAndAnime[streamInfo.Service].ContainsKey(streamInfo.AnimeName))
                    {
                        List<CsvRow> rowsForThisServiceAndAnime = rowsByServiceAndAnime[streamInfo.Service][streamInfo.AnimeName];

                        // Amazon URLs look like https://www.amazon.com/Our-Eyes-Finally-Met-Anothers/dp/B06Y5WC21S
                        // The "Our-Eyes-Finally-Met-Anothers" relates to an episode title.
                        // The B06Y5WC21S is some sort of ID that seems to relate to the episode rather than the whole series.
                        // For reasons unknown, the episode that represents the whole series can change, resulting in the URL changing.
                        // This results in a fair amount of churn in the CSV, around 20 changes per week.
                        // To avoid having to remap those streams to MAL IDs, use the following logic:

                        // If amazon service, and only one URL present in existing CSV for this (service, title),
                        // and only one url present in the streams that we just got, then consider the existing CSV rows
                        // a match and use their MAL IDs.

                        // Even if that is not the case, then only consider the ID at the end when matching URLs of streams
                        // to existing CSV rows.

                        if (streamInfo.Service != StreamingService.AmazonPrime)
                        {
                            existingCsvRows = rowsForThisServiceAndAnime.Where(row => row.Url == streamInfo.Url).ToList();
                        }
                        else
                        {
                            if (rowsForThisServiceAndAnime.GroupBy(row => row.Url).Count() == 1 && streamsByServiceAndAnime[streamInfo.Service][streamInfo.AnimeName].GroupBy(stream => stream.Url).Count() == 1)
                            {
                                existingCsvRows = rowsForThisServiceAndAnime.ToList();
                            }
                            else
                            {
                                string amazonStreamID = GetAmazonIDFromUrl(streamInfo.Url);
                                existingCsvRows = rowsForThisServiceAndAnime.Where(row => GetAmazonIDFromUrl(row.Url) == amazonStreamID).ToList();
                            }
                        }
                    }

                    if (existingCsvRows.Count > 0)
                    {
                        foreach (CsvRow existingRow in existingCsvRows)
                        {
                            // not WriteLine() - this should be \r\n regardless of what platform this is run on per the CSV RFC
                            // Header row did not write a newline, so there won't be a blank line between header and first row.
                            output.Write("\r\n");
                            output.Write("{0},{1},{2},{3}", QuoteForCsv(streamInfo.Service.ToString()),
                                QuoteForCsv(streamInfo.AnimeName), QuoteForCsv(streamInfo.Url),
                                existingRow.MalAnimeId.ToString());
                        }
                    }
                    else
                    {
                        output.Write("\r\n");

                        // Notice the comma at the end - leave MAL anime id blank for a human operator to fill in.
                        output.Write("{0},{1},{2},", QuoteForCsv(streamInfo.Service.ToString()), QuoteForCsv(streamInfo.AnimeName),
                            QuoteForCsv(streamInfo.Url));
                    }
                }
            }
        }

        // https://www.amazon.com/Our-Eyes-Finally-Met-Anothers/dp/B06Y5WC21S -> B06Y5WC21S
        static string GetAmazonIDFromUrl(string url)
        {
            int indexOfLastSlash = url.LastIndexOf('/');
            if (indexOfLastSlash == -1)
            {
                throw new Exception(string.Format("Amazon URL {0} doesn't have a slash in it.", url));
            }

            int idLength = url.Length - indexOfLastSlash - 1;
            if (idLength <= 0)
            {
                throw new Exception(string.Format("Length of ID in Amazon URL {0} is <= 0", url));
            }

            return url.Substring(indexOfLastSlash + 1, url.Length - indexOfLastSlash - 1);
        }

        static List<CsvRow> LoadCsv(string inputFile)
        {
            List<CsvRow> inputCsvRows = new List<CsvRow>();

            var csvConfig = new CsvHelper.Configuration.Configuration()
            {
                AllowComments = false,
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                IncludePrivateMembers = true,
                TrimOptions = CsvHelper.Configuration.TrimOptions.None,
            };

            using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (TextReader inputReader = new StreamReader(inputStream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(inputReader, csvConfig))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    StreamingService service = csvReader.GetField<StreamingService>("Service");
                    string animeName = csvReader.GetField<string>("Anime");
                    string url = csvReader.GetField<string>("URL");
                    string malIdString = csvReader.GetField<string>("MAL ID (or n/a)");

                    MalId malId;

                    // could be blank
                    if (string.IsNullOrWhiteSpace(malIdString))
                    {
                        malId = new MalId(malAnimeId: null, specified: false);
                    }
                    else if (malIdString.Equals("n/a", StringComparison.OrdinalIgnoreCase))
                    {
                        // n/a means the stream does not correspond to a MAL anime. Maybe it's a stream of anime reviews
                        // or maybe it's something incredibly obscure.
                        malId = new MalId(malAnimeId: null, specified: true);
                    }
                    else
                    {
                        int malIdInt = int.Parse(malIdString);
                        malId = new MalId(malAnimeId: malIdInt, specified: true);
                    }

                    CsvRow row = new CsvRow(service, animeName, url, malId);
                    inputCsvRows.Add(row);
                }
            }

            return inputCsvRows;
        }

        static void GenerateSql(string inputFile, string outputFile)
        {
            List<CsvRow> inputCsvRows = LoadCsv(inputFile);
            if (inputCsvRows.Any(row => !row.MalAnimeId.Specified))
            {
                throw new Exception("All rows must have a MAL anime id specified (or n/a).");
            }

            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (StreamWriter output = new StreamWriter(outputStream, Encoding.UTF8))
            {
                IEnumerable<streaming_service_anime_map> streamMaps = inputCsvRows
                    .Where(csvRow => csvRow.MalAnimeId.MalAnimeId.HasValue)
                    .Select(csvRow => new streaming_service_anime_map(
                        _mal_anime_id: csvRow.MalAnimeId.MalAnimeId.Value,
                        _streaming_service_id: (int)csvRow.Service,
                        _streaming_url: csvRow.Url
                    ));

                string sql = streaming_service_anime_map.CreateRefreshStreamMapSql(streamMaps);
                output.Write(sql);
            }
        }

        static string QuoteForCsv(string str)
        {
            // Enclose in quotes and replace quote with quote-quote
            return "\"" + str.Replace("\"", "\"\"") + "\"";
        }

        static List<IAnimeStreamInfoSource> GetStreamInfoSources(CommandLineArgs args, IWebClient webClient, IWebClient funimationWebClient)
        {
            IAnimeStreamInfoSource crunchyrollSource;
            if (args.CrunchyrollLocalHtmlPath != null)
            {
                crunchyrollSource = new CrunchyrollLocalHtmlStreamInfoSource(args.CrunchyrollLocalHtmlPath);
            }
            else
            {
                string crunchyrollUsername = GetCrunchyrollUsername();
                string crunchyrollPassword = GetCrunchyrollPassword();
                crunchyrollSource = new CrunchyrollStreamInfoSource(crunchyrollUsername, crunchyrollPassword, webClient);
            }

            return new List<IAnimeStreamInfoSource>()
            {
                // Crunchyroll first because it prompts for a username and password if not loading from a file. Let the user enter that first instead of in the middle.
                crunchyrollSource,
                new AmazonPrimeStreamInfoSource(webClient),
                new FunimationStreamInfoSource(funimationWebClient),
                new VizStreamInfoSource(webClient),
                new HuluStreamInfoSource(webClient),
                new ViewsterStreamInfoSource(webClient),
                new HidiveStreamInfoSource(webClient),
            };
        }

        static string GetCrunchyrollUsername()
        {
            Console.WriteLine("Crunchyroll username:");
            string username = Console.ReadLine();
            return username;
        }

        static string GetCrunchyrollPassword()
        {
            Console.WriteLine("Crunchyroll password:");
            string password = ReadPassword();
            return password;
        }

        static string ReadPassword()
        {
            // If Console.ReadKey returns a ConsoleKeyInfo with KeyChar and Key of 0 on Mono, stdin or stdout is redirected.
            // stdout being redirected affecting the value of Console.ReadKey is a bug (https://bugzilla.xamarin.com/show_bug.cgi?id=12552).
            // Returning 0 when stdin is redirected is also a bug (https://bugzilla.xamarin.com/show_bug.cgi?id=12551).
            // The documented behavior is to throw an InvalidOperationException.

            bool runningOnMono = Type.GetType("Mono.Runtime") != null;
            StringBuilder textEntered = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo key;
                try
                {
                    key = Console.ReadKey(intercept: true);
                }
                catch (InvalidOperationException)
                {
                    // Console.ReadKey throws InvalidOperationException if stdin is not a console
                    // .NET 4.5 provides Console.IsInputRedirected.
                    // Switch to 4.5 once Linux distros start packaging a Mono version that support 4.5.
                    throw new Exception("Cannot prompt for password because stdin is redirected.");
                }
                if (runningOnMono && key.KeyChar == '\0' && (int)key.Key == 0)
                {
                    throw new Exception("Cannot prompt for password because stdin is redirected.");
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (textEntered.Length > 0)
                    {
                        textEntered.Length = textEntered.Length - 1;
                    }
                }
                else
                {
                    char c = key.KeyChar;
                    textEntered.Append(c);
                }
            }

            return textEntered.ToString();
        }
    }
}
