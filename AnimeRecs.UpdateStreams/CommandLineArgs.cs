using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;
using System.IO;

namespace AnimeRecs.UpdateStreams
{
    internal enum Operation
    {
        CreateCsv,
        GenerateSql
    }
    
    internal class CommandLineArgs
    {
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } private set { m_showHelp = value; } }

        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }
        public string CrunchyrollLocalHtmlPath { get; private set; }

        private Operation m_op = Operation.CreateCsv;
        public Operation Op { get { return m_op; } set { m_op = value; } }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "i|input=", "Input file. Omit to create a new csv.", arg => InputFile = arg },
                { "o|output=", "Output file.", arg => OutputFile = arg },
                { "crunchyroll-local-html-file=",
                    "(optional) Path to a local saved html file of http://www.crunchyroll.com/videos/anime/alpha?group=all. Use this if Crunchyroll has Cloudfare's \"I'm under attack\" mode on where it checks your browser to try to tell if it's from a real browser or not. If that mode is on and this option is not passed, you will get \"Error logging in to crunchroll, HTTP status: Service Temporarily Unavailable\".",
                    arg => CrunchyrollLocalHtmlPath = arg },
                { "sql", "Generate SQL from the input csv file. All mappings in the file are expected to be completed.", argExistence => { if(argExistence != null) Op = Operation.GenerateSql; } }
            };

            return optionSet;
        }

        public CommandLineArgs(string[] args)
        {
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            if (ShowHelp)
            {
                DisplayHelp(Console.Out);
                Environment.Exit(0);
            }

            if (OutputFile == null)
            {
                throw new OptionException("Output file not specified.", "o");
            }

            if (Op == Operation.GenerateSql && InputFile == null)
            {
                throw new OptionException("Input file not specified.", "i");
            }
        }

        public void DisplayHelp(TextWriter writer)
        {
            writer.WriteLine("Usage: [OPTIONS]");
            writer.WriteLine();
            writer.WriteLine("Parameters:");
            GetOptionSet().WriteOptionDescriptions(writer);
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
