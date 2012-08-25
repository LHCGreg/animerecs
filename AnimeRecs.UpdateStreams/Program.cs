using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using MiscUtil.Collections.Extensions;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
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

            if (commandLine.Op == Operation.CreateCsv)
            {
                CreateCsv(commandLine.InputFile, commandLine.OutputFile);
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
        static void CreateCsv(string inputFile, string outputFile)
        {
            List<CsvRow> inputCsvRows;
            if (inputFile == null)
            {
                inputCsvRows = new List<CsvRow>();
            }
            else
            {
                inputCsvRows = LoadCsv(inputFile);
            }

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

            using (StreamWriter output = new StreamWriter(outputFile))
            {
                List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();

                List<IAnimeStreamInfoSource> streamInfoSources = GetStreamInfoSources();

                foreach (IAnimeStreamInfoSource streamInfoSource in streamInfoSources)
                {
                    ICollection<AnimeStreamInfo> streamsFromThisSource = streamInfoSource.GetAnimeStreamInfo();
                    streams.AddRange(streamsFromThisSource);
                }

                string header = "Service,Anime,URL,Subscription Required?,MAL ID (or n/a)";
                output.Write(header);

                foreach (AnimeStreamInfo streamInfo in streams.OrderBy(stream => stream.Service).ThenBy(stream => stream.AnimeName).ThenBy(stream => stream.SubscriptionRequired).ThenBy(stream => stream.Url))
                {
                    List<CsvRow> existingCsvRows = new List<CsvRow>();
                    if (rowsByServiceAndAnime.ContainsKey(streamInfo.Service) && rowsByServiceAndAnime[streamInfo.Service].ContainsKey(streamInfo.AnimeName))
                    {
                        List<CsvRow> rowsForThisServiceAndAnime = rowsByServiceAndAnime[streamInfo.Service][streamInfo.AnimeName];
                        existingCsvRows = rowsForThisServiceAndAnime.Where(row => row.Url == streamInfo.Url && row.SubscriptionRequired == streamInfo.SubscriptionRequired).ToList();
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
                            output.Write("{0},{1},{2},{3},{4}", QuoteForCsv(existingRow.Service.ToString()),
                                QuoteForCsv(existingRow.AnimeName), QuoteForCsv(existingRow.Url),
                                QuoteForCsv(existingRow.SubscriptionRequired.ToString()), existingRow.MalAnimeId.ToString());
                        }
                    }
                    else
                    {
                        output.Write("{0},{1},{2},{3},", QuoteForCsv(streamInfo.Service.ToString()), QuoteForCsv(streamInfo.AnimeName),
                            QuoteForCsv(streamInfo.Url), QuoteForCsv(streamInfo.SubscriptionRequired.ToString()));
                    }
                }
            }
        }

        static List<CsvRow> LoadCsv(string inputFile)
        {
            List<CsvRow> inputCsvRows = new List<CsvRow>();
            using (TextFieldParser csvReader = new TextFieldParser(inputFile))
            {
                csvReader.TextFieldType = FieldType.Delimited;
                csvReader.HasFieldsEnclosedInQuotes = true;
                csvReader.Delimiters = new string[] { "," };

                // Skip header line
                csvReader.ReadFields();

                while (!csvReader.EndOfData)
                {
                    string[] fields = csvReader.ReadFields();
                    if (fields.Length == 0)
                    {
                        continue;
                    }

                    string serviceString = fields[0];
                    string animeName = fields[1];
                    string url = fields[2];
                    string subscriptionRequiredString = fields[3];
                    string malIdString = fields[4];

                    StreamingService service = (StreamingService)Enum.Parse(typeof(StreamingService), serviceString);
                    bool subscriptionRequired = bool.Parse(subscriptionRequiredString);
                    MalId malId;

                    // could be blank
                    if (string.IsNullOrWhiteSpace(malIdString))
                    {
                        malId = new MalId(malAnimeId: null, specified: false);
                    }
                    else if (malIdString.Equals("n/a", StringComparison.OrdinalIgnoreCase))
                    {
                        malId = new MalId(malAnimeId: null, specified: true);
                    }
                    else
                    {
                        int malIdInt = int.Parse(malIdString);
                        malId = new MalId(malAnimeId: malIdInt, specified: true);
                    }

                    inputCsvRows.Add(new CsvRow(service: service, animeName: animeName, url: url, subscriptionRequired: subscriptionRequired, malAnimeId: malId));
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
                        _requires_subscription: csvRow.SubscriptionRequired,
                        _streaming_url: csvRow.Url
                    ));

                string sql = streaming_service_anime_map.CreateRefreshStreamMapSql(streamMaps);
                output.Write(sql);
            }
        }

        static string QuoteForCsv(string str)
        {
            return "\"" + str.Replace("\"", "\"\"") + "\"";
        }

        static List<IAnimeStreamInfoSource> GetStreamInfoSources()
        {
            List<IAnimeStreamInfoSource> streamInfoSources = new List<IAnimeStreamInfoSource>();
            streamInfoSources.Add(new CrunchyrollStreamInfoSource());
            return streamInfoSources;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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