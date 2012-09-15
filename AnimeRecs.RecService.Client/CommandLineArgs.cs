using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Client
{
    internal class CommandLineArgs
    {
        private HashSet<string> AllowedCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            OpNames.Ping,
            OpNames.LoadRecSource,
            OpNames.UnloadRecSource,
            OpNames.GetRecSourceType,
            OpNames.ReloadTrainingData,
            OpNames.GetMalRecs,
            OpNames.FinalizeRecSources,
            "Raw"
        };

        private HashSet<string> AllowedRecSourceTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            RecSourceTypes.AverageScore,
            RecSourceTypes.MostPopular,
            RecSourceTypes.AnimeRecs,
            RecSourceTypes.BiasedMatrixFactorization
        };

        private bool m_showHelp = false;
        public bool ShowHelp { get { return m_showHelp; } set { m_showHelp = value; } }

        public string Operation { get; private set; }

        private string m_pingMessage = "ping";
        public string PingMessage { get { return m_pingMessage; } set { m_pingMessage = value; } }

        private int m_portNumber = 5541;
        public int PortNumber { get { return m_portNumber; } set { m_portNumber = value; } }

        private string m_recSourceName = "default";
        public string RecSourceName { get { return m_recSourceName; } set { m_recSourceName = value; } }

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
                if (m_useDropped.HasValue)
                    return m_useDropped.Value;
                else
                {
                    if (RecSourceType.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
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

        public string MalUsername { get; private set; }

        private int m_numRecs = 50;
        public int NumRecs { get { return m_numRecs; } set { m_numRecs = value; } }

        private decimal m_targetScore = 8m;
        public decimal TargetScore { get { return m_targetScore; } set { m_targetScore = value; } }

        private BiasedMatrixFactorizationRecSourceParams m_biasedMatrixFactorizationParams = new BiasedMatrixFactorizationRecSourceParams();
        public BiasedMatrixFactorizationRecSourceParams BiasedMatrixFactorizationParams
        {
            get { return m_biasedMatrixFactorizationParams; }
            set { m_biasedMatrixFactorizationParams = value; }
        }

        private ReloadBehavior m_reloadMode = ReloadBehavior.HighAvailability;
        public ReloadBehavior ReloadMode { get { return m_reloadMode; } set { m_reloadMode = value; } }

        private bool m_finalize = false;
        public bool Finalize { get { return m_finalize; } set { m_finalize = value; } }

        public string RawJson { get; set; }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "c|command=", "Command. Possible commands are Ping, LoadRecSource, GetRecSourceType, GetMalRecs, ReloadTrainingData, FinalizeRecSources, and UnloadRecSource.", arg => SetCommand(arg) },
                { "p|port=", "Port the rec service is listening on. Defaults to 5541.", arg => PortNumber = int.Parse(arg) },
                { "ping_message=", "Message to send with a ping command. Used with the Ping command. Defaults to \"ping\".", arg => PingMessage = arg },
                { "finalize", "Finalize the rec sources loaded after the reload is complete to reduce memory usage. Used with the ReloadTrainingData command.", argExistence => Finalize = (argExistence != null) },
                { "name|rec_source_name=", "Rec source name. Used with the LoadRecSource, GetRecSourceType, UnloadRecSource, and GetMalRecs commands. Defaults to \"default\"", arg => RecSourceName = arg },
                { "reload_mode=", "Used with ReloadTrainingData. Possible values are HighAvailability and LowMemory. Defaults to HighAvailability. HighAvailability: Keep the old training data and rec sources in memory while the reload/retrain is going on to keep the rec service serving requests. Requires around twice the amount of memory normally consumed. LowMemory: Drop the old training data and rec sources before starting the reload/retrain. This avoids using double normal memory but means the rec service cannot give recommendations while the reload/retrain is going on.", arg => SetReloadMode(arg) },
                { "f", "Replace an existing rec source. Used with the LoadRecSource command", argExistence => ReplaceExistingRecSource = (argExistence != null) },
                { "type|rec_source_type=", "Rec source type. Required for LoadRecSource command", arg => SetRecSourceType(arg) },
                { "min_episodes_to_count_incomplete=", "Minimum episodes to count the rating of a show a user is currently watched. Used with the LoadRecSource command with the AverageScore, MostPopular, and AnimeRecs rec source types. Defaults to 26.",
                    arg => { MinEpisodesToCountIncomplete = int.Parse(arg); BiasedMatrixFactorizationParams.MinEpisodesToCountIncomplete = int.Parse(arg); } },
                { "use_dropped", "Count dropped anime. Used with the LoadRecSource command with the AverageScore and MostPopular rec source types. Defaults to false for MostPopular, true for everything else.",
                    argExistence => { UseDropped = (argExistence != null); BiasedMatrixFactorizationParams.UseDropped = (argExistence != null); } },
                { "min_users_to_count_anime=", "Minimum users to have seen an anime for it to be considered by the recommendation algorithm. Used by the LoadRecSource command with the AverageScore and BiasedMatrixFactorization rec source types. Defaults to 50.",
                    arg => { MinUsersToCountAnime = int.Parse(arg); BiasedMatrixFactorizationParams.MinUsersToCountAnime = int.Parse(arg); } },
                { "num_recommenders_to_use=", "Number of recommenders to use for the AnimeRecs rec source. Used by the LoadRecSource command with the AnimeRecs rec source type. Defaults to 100.",
                    arg => NumRecommendersToUse = int.Parse(arg) },
                { "percent_recommended=", "Percentage of anime seen that a recommender recommends. Used by the LoadRecSource command with the AnimeRecs rec source type. Defaults to 35.",
                    arg => FractionRecommended = double.Parse(arg) / 100 },
                { "u|username=", "MAL username. Required for the GetMalRecs command.", arg => MalUsername = arg },
                { "n|num_recs=", "Number of recommendations to get. Used by the GetMalRecs command. Defaults to 50.", arg => NumRecs = int.Parse(arg) },
                { "t|target_score=", "Target score. Used with the GetMalRecs command. Only used by some rec sources. Defaults to 8.", arg => TargetScore = decimal.Parse(arg) },
                { "bias_learn_rate=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 1.", arg => BiasedMatrixFactorizationParams.BiasLearnRate = float.Parse(arg) },
                { "bias_reg=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 0.01.", arg => BiasedMatrixFactorizationParams.BiasReg = float.Parse(arg) },
                { "bold_driver", "Used when loading a BiasedMatrixFactorization rec source. Defaults to false.", argExistence => BiasedMatrixFactorizationParams.BoldDriver = (argExistence != null) },
                { "frequency_regularization", "Used when loading a BiasedMatrixFactorization rec source. Defaults to false.", argExistence => BiasedMatrixFactorizationParams.FrequencyRegularization = (argExistence != null) },
                { "learn_rate=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 0.01.", arg => BiasedMatrixFactorizationParams.LearnRate = float.Parse(arg) },
                { "optimization_target=", "Used when loading a BiasedMatrixFactorization rec source. Must be LogisticLoss, MAE, or RMSE. Defaults to RMSE.", arg => SetOptimizationTarget(arg) },
                { "num_factors=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 10.", arg => BiasedMatrixFactorizationParams.NumFactors = uint.Parse(arg) },
                { "num_iter=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 30.", arg => BiasedMatrixFactorizationParams.NumIter = uint.Parse(arg) },
                { "reg_i=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 0.015.", arg => BiasedMatrixFactorizationParams.RegI = float.Parse(arg) },
                { "reg_u=", "Used when loading a BiasedMatrixFactorization rec source. Defaults to 0.015.", arg => BiasedMatrixFactorizationParams.RegU = float.Parse(arg) },
                   
                { "<>", arg => SetRaw(arg) },
            };

            return optionSet;
        }

        private void SetCommand(string command)
        {
            if (AllowedCommands.Contains(command))
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
            if (AllowedRecSourceTypes.Contains(type))
            {
                RecSourceType = type;
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a recognized rec source type.", type), "rec_source_type");
            }
        }

        private void SetOptimizationTarget(string target)
        {
            if (target.Equals("LogisticLoss", StringComparison.OrdinalIgnoreCase)
                || target.Equals("MAE", StringComparison.OrdinalIgnoreCase)
                || target.Equals("RMSE", StringComparison.OrdinalIgnoreCase))
            {
                BiasedMatrixFactorizationParams.OptimizationTarget = target;
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a recognized optimization target.", target), "optimization_target");
            }
        }

        private void SetReloadMode(string mode)
        {
            if (!Enum.TryParse<ReloadBehavior>(mode, out m_reloadMode))
            {
                throw new OptionException(string.Format("{0} is not a recognized reload mode.", mode), "reload_mode");
            }
        }

        private void SetRaw(string rawJson)
        {
            if (Operation != null && Operation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
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

            if (!ShowHelp && Operation.Equals(OpNames.LoadRecSource, StringComparison.OrdinalIgnoreCase) && RecSourceType == null)
            {
                throw new OptionException("Rec source type was not specified", "rec_source_type");
            }

            if (!ShowHelp && Operation.Equals(OpNames.GetMalRecs, StringComparison.OrdinalIgnoreCase) && MalUsername == null)
            {
                throw new OptionException("MAL username was not specified.", "username");
            }

            if (!ShowHelp && Operation.Equals("raw", StringComparison.OrdinalIgnoreCase) && RawJson == null)
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