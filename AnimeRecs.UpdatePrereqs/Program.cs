using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using AnimeRecs.DAL;
using CsvHelper;

namespace AnimeRecs.UpdatePrereqs
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);

            List<mal_anime_prerequisite> prereqs = new List<mal_anime_prerequisite>();

            // open input file and parse it

            var csvConfig = new CsvHelper.Configuration.Configuration()
            {
                AllowComments = false,
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                IncludePrivateMembers = true,
                TrimOptions = CsvHelper.Configuration.TrimOptions.None,
            };

            using (FileStream inputStream = new FileStream(commandLine.InputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (TextReader inputReader = new StreamReader(inputStream, Encoding.UTF8))
            using (CsvReader csvReader = new CsvReader(inputReader, csvConfig))
            {
                while (csvReader.Read())
                {
                    string prereqMalUrl = csvReader.GetField<string>("Prerequisite");
                    string animeMalUrl = csvReader.GetField<string>("Anime");

                    int prereqId = GetMalIdFromMalUrl(prereqMalUrl);
                    int animeId = GetMalIdFromMalUrl(animeMalUrl);

                    mal_anime_prerequisite prereq = new mal_anime_prerequisite(
                        _mal_anime_id: animeId,
                        _prerequisite_mal_anime_id: prereqId
                    );

                    prereqs.Add(prereq);
                }
            }

            string sql = mal_anime_prerequisite.CreateRefreshPrerequisiteMapSql(prereqs);
            using (FileStream outputStream = new FileStream(commandLine.OutputFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (StreamWriter output = new StreamWriter(outputStream, Encoding.UTF8))
            {
                output.Write(sql);
            }
        }

        static Regex MalAnimeUrlRegex = new Regex(@"http://myanimelist.net/anime/(?<MalId>\d+)");

        static int GetMalIdFromMalUrl(string malUrl)
        {
            Match m = MalAnimeUrlRegex.Match(malUrl);
            if (!m.Success)
            {
                throw new Exception(string.Format("URL {0} does not match.", malUrl));
            }
            return int.Parse(m.Groups["MalId"].Value);
        }
    }
}
