using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;
using System.IO;

namespace AnimeRecs.UpdatePrereqs
{
    class CommandLineArgs
    {
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } private set { m_showHelp = value; } }

        public string InputFilePath { get; private set; }

        private string m_outputFilePath;
        public string OutputFilePath
        {
            get
            {
                if (m_outputFilePath != null)
                {
                    return m_outputFilePath;
                }
                else if (InputFilePath == null)
                {
                    return null;
                }
                else
                {
                    return Path.ChangeExtension(InputFilePath, ".sql");
                }
            }
            set
            {
                m_outputFilePath = value;
            }
        }
        
        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "o|output=", "Output sql file. If not set, defaults to the input file with a .sql extension instead of whatever extension it has.", arg => OutputFilePath = arg },
                { "<>", "Input csv file", arg => InputFilePath = arg }
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

            if (InputFilePath == null)
            {
                throw new OptionException("No input csv file specified.", "<>");
            }
        }

        public void DisplayHelp(TextWriter writer)
        {
            writer.WriteLine("Usage: [OPTIONS] <input csv file>");
            writer.WriteLine();
            writer.WriteLine("Parameters:");
            GetOptionSet().WriteOptionDescriptions(writer);
        }
    }
}
