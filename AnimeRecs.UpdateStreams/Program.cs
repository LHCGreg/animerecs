using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using MiscUtil.Collections.Extensions;
using AnimeRecs.DAL;
using AnimeRecs.UpdateStreams.Crunchyroll;

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

            List<IAnimeStreamInfoSource> streamInfoSources = GetStreamInfoSources(args);
            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            foreach (IAnimeStreamInfoSource streamInfoSource in streamInfoSources)
            {
                ICollection<AnimeStreamInfo> streamsFromThisSource = streamInfoSource.GetAnimeStreamInfo();
                streams.AddRange(streamsFromThisSource);
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
            using (StreamWriter output = new StreamWriter(outputFile))
            {
                string header = "Service,Anime,URL,MAL ID (or n/a)";
                output.Write(header);

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

                        if (streamInfo.Service != StreamingService.AmazonAnimeStrike && streamInfo.Service != StreamingService.AmazonPrime)
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

                    output.Write("\r\n"); // not WriteLine() - this should be \r\n regardless of what platform this is run on per the CSV RFC

                    if (existingCsvRows.Count > 0)
                    {
                        foreach (var existingRowIt in existingCsvRows.AsSmartEnumerable())
                        {
                            CsvRow existingRow = existingRowIt.Value;
                            if (!existingRowIt.IsFirst)
                            {
                                output.Write("\r\n");
                            }
                            output.Write("{0},{1},{2},{3}", QuoteForCsv(streamInfo.Service.ToString()),
                                QuoteForCsv(streamInfo.AnimeName), QuoteForCsv(streamInfo.Url),
                                existingRow.MalAnimeId.ToString());
                        }
                    }
                    else
                    {
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
            using (TextFieldParser csvReader = new TextFieldParser(inputFile))
            {
                csvReader.TextFieldType = FieldType.Delimited;
                csvReader.HasFieldsEnclosedInQuotes = true;
                csvReader.Delimiters = new string[] { "," };
                csvReader.TrimWhiteSpace = false;

                // Skip header line
                csvReader.ReadFields();

                while (!csvReader.EndOfData)
                {
                    string[] fields = csvReader.ReadFields();
                    // Skip blank lines
                    if (fields.Length == 0)
                    {
                        continue;
                    }

                    string serviceString = fields[0];
                    if (string.IsNullOrWhiteSpace(serviceString))
                    {
                        continue; // Skip blank lines
                    }

                    string animeName = fields[1];
                    string url = fields[2];
                    string malIdString = fields[3];

                    StreamingService service = (StreamingService)Enum.Parse(typeof(StreamingService), serviceString);
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

                    inputCsvRows.Add(new CsvRow(service: service, animeName: animeName, url: url, malAnimeId: malId));
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

            using (StreamWriter output = new StreamWriter(outputFile))
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

        static List<IAnimeStreamInfoSource> GetStreamInfoSources(CommandLineArgs args)
        {
            IAnimeStreamInfoSource crunchyrollSource;
            if (args.CrunchyrollLocalHtmlPath != null)
            {
                crunchyrollSource = new CrunchyrollLocalHtmlStreamInfoSource(args.CrunchyrollLocalHtmlPath);
            }
            else
            {
                crunchyrollSource = new CrunchyrollStreamInfoSource();
            }

            return new List<IAnimeStreamInfoSource>()
            {
                // Crunchyroll first because it prompts for a username and password if not loading from a file. Let the user enter that first instead of in the middle.
                crunchyrollSource,
                new AmazonAnimeStrikeStreamInfoSource(),
                new AmazonPrimeStreamInfoSource(),
                new FunimationStreamInfoSource(),
                new VizStreamInfoSource(),
                new HuluStreamInfoSource(),
                new ViewsterStreamInfoSource(),
                new DaisukiStreamInfoSource(),
                new AnimeNetworkStreamInfoSource()
            };
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.