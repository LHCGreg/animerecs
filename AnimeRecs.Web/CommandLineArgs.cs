using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Options;

namespace AnimeRecs.Web
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
