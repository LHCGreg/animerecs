using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdatePrereqs
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);

            List<mal_anime_prerequisite> prereqs = new List<mal_anime_prerequisite>();

            // open input file and parse it
            using (TextFieldParser csvParser = new TextFieldParser(commandLine.InputFilePath))
            {
                csvParser.TextFieldType = FieldType.Delimited;
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.Delimiters = new string[] { "," };
                bool headerRead = false;
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    if (!headerRead)
                    {
                        headerRead = true;
                        continue;
                    }

                    string prereqMalUrl = fields[0];
                    string animeMalUrl = fields[1];

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
            using (TextWriter output = new StreamWriter(commandLine.OutputFilePath))
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

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdatePrereqs
//
// AnimeRecs.UpdatePrereqs is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdatePrereqs is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdatePrereqs.  If not, see <http://www.gnu.org/licenses/>.