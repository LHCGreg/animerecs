using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Options;

namespace AnimeRecs.RecService
{
    internal class CommandLineArgs
    {
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } private set { m_showHelp = value; } }

        private int m_portNumber = 5541;
        public int PortNumber { get { return m_portNumber; } private set { m_portNumber = value; } }

        private string m_configFile = "config.xml";
        public string ConfigFile { get { return m_configFile; } private set { m_configFile = value; } }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "p|port=", "Port to listen on. Defaults to 5541.", arg => PortNumber = int.Parse(arg) },
                { "f|config=", "File to load configuration settings from. Defaults to config.xml.", arg => ConfigFile = arg }
            };

            return optionSet;
        }

        public CommandLineArgs(string[] args)
        {
            Logging.Log.Debug("Parsing command line args.");
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            Logging.Log.DebugFormat("Command line args parsed. PortNumber={0}, ConfigFile={1}, ShowHelp={2}", PortNumber, ConfigFile, ShowHelp);
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
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.