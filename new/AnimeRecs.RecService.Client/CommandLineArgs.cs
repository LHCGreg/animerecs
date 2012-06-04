using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Client
{
    class CommandLineArgs
    {
        private HashSet<string> AllowedCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            OpNames.Ping,
            OpNames.LoadRecSource,
            OpNames.ReloadTrainingData,
            "Raw"
        };

        private HashSet<string> AllowedRecSourceTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            RecSourceTypes.AverageScore,
            RecSourceTypes.MostPopular
        };
        
        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } set { m_showHelp = value; } }

        public string Operation { get; private set; }

        private string m_pingMessage = "ping";
        public string PingMessage { get { return m_pingMessage; } set { m_pingMessage = value; } }

        private int m_portNumber = 5541;
        public int PortNumber { get { return m_portNumber; } set { m_portNumber = value; } }

        public string RecSourceName { get; set; }

        private bool m_replaceExistingRecSource = false;
        public bool ReplaceExistingRecSource { get { return m_replaceExistingRecSource; } set { m_replaceExistingRecSource = value; } }

        public string RecSourceType { get; private set; }

        private int m_minEpisodesToCountIncomplete = 26;
        public int MinEpisodesToCountIncomplete { get { return m_minEpisodesToCountIncomplete; } set { m_minEpisodesToCountIncomplete = value; } }

        private bool? m_useDropped = null;
        public bool UseDropped
        {
            get
            {
                if(m_useDropped.HasValue)
                    return m_useDropped.Value;
                else
                {
                    if(RecSourceType.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
                        return false;
                    else
                        return true;
                }
            }
            set
            {
                m_useDropped = value;
            }
        }

        private int m_minUsersToCountAnime = 50;
        public int MinUsersToCountAnime { get { return m_minUsersToCountAnime; } set { m_minUsersToCountAnime = value; } }

        private int m_numRecommendersToUse = 100;
        public int NumRecommendersToUse { get { return m_numRecommendersToUse; } set { m_numRecommendersToUse = value; } }

        private double m_fractionRecommended = 0.35;
        public double FractionRecommended { get { return m_fractionRecommended; } set { m_fractionRecommended = value; } }

        public string RawJson { get; set; }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "c|command=", "Command", arg => SetCommand(arg) },
                { "p|port=", "Port the rec service is listening on. Defaults to 5541.", arg => PortNumber = int.Parse(arg) },
                { "ping_message=", "Message to send with a ping command. Only has meaning when the ping command is used. Defaults to \"ping\".", arg => PingMessage = arg },
                { "rec_source_name=", "Rec source name. Required for LoadRecSource command.", arg => RecSourceName = arg },
                { "f", "Replace an existing rec source.", argExistence => ReplaceExistingRecSource = (argExistence != null) },
                { "rec_source_type=", "Rec source type. Required for LoadRecSource command", arg => SetRecSourceType(arg) },
                { "min_episodes_to_count_incomplete=", "Minimum episodes to count the rating of a show a user is currently watched. Used with AverageScore, MostPopular, and AnimeRecs. Defaults to 26.",
                    arg => MinEpisodesToCountIncomplete = int.Parse(arg) },
                { "use_dropped", "Count dropped anime. Used with AverageScore and MostPopular. Defaults to false for MostPopular, true for everything else.",
                    argExistence => UseDropped = (argExistence != null) },
                { "min_users_to_count_anime=", "Minimum users to have seen an anime for it to be considered by the recommendation algorithm. Used with AverageScore. Defaults to 50.",
                    arg => MinUsersToCountAnime = int.Parse(arg) },
                { "num_recommenders_to_use=", "Number of recommenders to use for the AnimeRecs rec source. Defaults to 100.",
                    arg => NumRecommendersToUse = int.Parse(arg) },
                { "percent_recommended=", "Percentage of anime seen that a recommender recommends. Defaults to 35.",
                    arg => FractionRecommended = double.Parse(arg) / 100 },
                { "<>", arg => SetRaw(arg) },
            };

            return optionSet;
        }

        private void SetCommand(string command)
        {
            if(AllowedCommands.Contains(command))
            {
                Operation = command;
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a valid command.", command), "command");
            }
        }

        private void SetRecSourceType(string type)
        {
            if(AllowedRecSourceTypes.Contains(type))
            {
                RecSourceType = type;
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a recognized rec source type.", type), "rec_source_type");
            }
        }

        private void SetRaw(string rawJson)
        {
            if(Operation != null && Operation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                RawJson = rawJson;
            }
            else
            {
                throw new OptionException(string.Format("Unrecognized option \"{0}\".", rawJson), "<>");
            }
        }

        public CommandLineArgs(string[] args)
        {
            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            if (!ShowHelp && Operation == null)
            {
                throw new OptionException("Command was not specified.", "command");
            }

            if(!ShowHelp && Operation.Equals(OpNames.LoadRecSource, StringComparison.OrdinalIgnoreCase) && RecSourceType == null)
            {
                throw new OptionException("Rec source type was not specified", "rec_source_type");
            }

            if(!ShowHelp && Operation.Equals(OpNames.LoadRecSource, StringComparison.OrdinalIgnoreCase) && RecSourceName == null)
            {
                throw new OptionException("Rec source name was not specified.", "rec_source_name");
            }

            if(!ShowHelp && Operation.Equals("raw", StringComparison.OrdinalIgnoreCase) && RawJson == null)
            {
                throw new OptionException("No raw json specified.", "<>");
            }
        }

        public void DisplayHelp(TextWriter writer)
        {
            writer.WriteLine("Usage: {0} [OPTIONS]", GetProgramName());
            writer.WriteLine();
            writer.WriteLine("Parameters:");
            GetOptionSet().WriteOptionDescriptions(writer);
        }

        public static string GetProgramName()
        {
            string[] argsWithProgramName = System.Environment.GetCommandLineArgs();
            string programName;
            if (argsWithProgramName[0].Equals(string.Empty))
            {
                // "If the file name is not available, the first element is equal to String.Empty."
                // Doesn't say why that would happen, but ok...
                programName = (new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Name) + ".exe";
            }
            else
            {
                programName = Path.GetFileName(argsWithProgramName[0]);
            }

            return programName;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.Client.
//
// AnimeRecs.RecService.Client is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.Client is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.Client.  If not, see <http://www.gnu.org/licenses/>.