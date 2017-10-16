using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Options;

namespace AnimeRecs.FreshenMalDatabase
{
    internal class CommandLineArgs
    {
        public bool ShowHelp { get; private set; } = false;
        public string ConfigFile { get; private set; } = "config.xml";

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "f|config=", "File to load configuration settings from. Defaults to config.xml.", arg => ConfigFile = arg }
            };

            return optionSet;
        }

        public CommandLineArgs(string[] args)
        {
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);
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
// This file is part of AnimeRecs.FreshenMalDatabase.
//
// AnimeRecs.FreshenMalDatabase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.FreshenMalDatabase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.FreshenMalDatabase.  If not, see <http://www.gnu.org/licenses/>.
