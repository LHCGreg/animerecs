using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Options;

namespace AnimeRecs.RecService
{
    internal class CommandLineArgs
    {
        public bool ShowHelp { get; private set; } = false;
        public int PortNumber { get; private set; } = 5541;
        public string ConfigFile { get; private set; } = "config.xml";

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
