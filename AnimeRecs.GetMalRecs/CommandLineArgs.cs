using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using System.IO;

namespace AnimeRecs.GetMalRecs
{
    enum RecommenderType
    {
        AverageRating,
        MostPopular,
        AnimeRecs,
        BiasedMatrixFactorization
    }
    
    class CommandLineArgs
    {
        private Dictionary<string, RecommenderType> CmdLineArgStringToRecType = new Dictionary<string, RecommenderType>(StringComparer.OrdinalIgnoreCase)
        {
            { "AverageRating", RecommenderType.AverageRating },
            { "MostPopular", RecommenderType.MostPopular },
            { "AnimeRecs", RecommenderType.AnimeRecs },
            { "BiasedMatrixFactorization", RecommenderType.BiasedMatrixFactorization }
        };
        
        public bool ShowHelp { get; private set; }

        private RecommenderType m_recommenderType = RecommenderType.AverageRating;
        public RecommenderType RecommenderType { get { return m_recommenderType; } set { m_recommenderType = value; } }

        private int m_minEpisodesToCountIncomplete = 26;
        public int MinEpisodesToCountIncomplete { get { return m_minEpisodesToCountIncomplete; } set { m_minEpisodesToCountIncomplete = value; } }

        private int m_minUsersToCountAnime = 50;
        public int MinUsersToCountAnime { get { return m_minUsersToCountAnime; } set { m_minUsersToCountAnime = value; } }

        private bool? m_useDropped = null;
        public bool UseDropped { get; private set; }

        private int m_numRecommenders = 100;
        public int NumRecommenders { get { return m_numRecommenders; } set { m_numRecommenders = value; } }

        private double m_fractionRecommended = 0.35;
        public double FractionRecommended { get { return m_fractionRecommended; } set { m_fractionRecommended = value; } }

        private double m_targetFraction = 0.35;
        public double TargetFraction { get { return m_targetFraction; } set { m_targetFraction = value; } }

        private int m_numRecs = 50;
        public int NumRecs { get { return m_numRecs; } set { m_numRecs = value; } }

        public string MalUser { get; private set; }

        public OptionSet GetOptionSet()
        {
            OptionSet optionSet = new OptionSet()
            {
                { "?|h|help", "Show this message and exit.", argExistence => ShowHelp = (argExistence != null) },
                { "recommender=", "Recommender to use. Choices are AverageRating, MostPopular, AnimeRecs, and BiasedMatrixFactorization.", arg => SetRecommender(arg) },
                { "minepisodes=", "Minimum number of episodes to count a rating for an anime a user is currently watching. Defaults to 26",
                    arg => MinEpisodesToCountIncomplete = int.Parse(arg) },
                { "minusers=", "Minimum users with a rating for an anime to consider the anime for recommendations. Only has effect for AverageRating. Defaults to 50.",
                    arg => MinUsersToCountAnime = int.Parse(arg) },
                { "withdropped", "Consider ratings on dropped anime as if they were completed. Defaults to false for MostPopular, true for AverageRating and BiasedMatrixFactorization.",
                    argExistence => m_useDropped = (argExistence != null) },
                { "numrecommenders=", "Number of users to use as recommenders. Only has effect for AnimeRecs. Defaults to 100.",
                    arg => NumRecommenders = int.Parse(arg) },
                { "percentrecommended=", "Percentage of a recommender's anime to recommend. Only has effect for AnimeRecs. Defaults to 35.",
                    arg => FractionRecommended = double.Parse(arg) / 100 },
                { "targetpercent=", "Percentage of user's anime to be considered a match if recommended. Only has effect for AnimeRecs. Defaults to 35.",
                    arg => TargetFraction = double.Parse(arg) / 100 },
                { "numrecs=", "Number of recommendations to display. Defaults to 50.", arg => NumRecs = int.Parse(arg) },
                { "user=", "MAL username of the user to get recommendations for. REQUIRED.", arg => MalUser = arg }
            };

            return optionSet;
        }

        private void SetRecommender(string recommenderString)
        {
            if (CmdLineArgStringToRecType.ContainsKey(recommenderString))
            {
                RecommenderType = CmdLineArgStringToRecType[recommenderString];
            }
            else
            {
                throw new OptionException(string.Format("{0} is not a valid recommender type.", recommenderString), "recommender");
            }
        }

        public CommandLineArgs(string[] args)
        {
            ShowHelp = false;

            OptionSet optionSet = GetOptionSet();
            optionSet.Parse(args);

            if (MalUser == null)
            {
                throw new OptionException("User was not specified.", "user");
            }
            
            if (m_useDropped == null && RecommenderType == GetMalRecs.RecommenderType.MostPopular)
            {
                UseDropped = false;
            }
            else if (m_useDropped == null)
            {
                UseDropped = true;
            }
        }

        public static void DisplayHelp(TextWriter writer)
        {
            CommandLineArgs emptyArgs = new CommandLineArgs(new string[] { }); // This is a bit of a hack, but I can't think of a better way to do it
            writer.WriteLine("Usage: {0} [OPTIONS]", GetProgramName());
            writer.WriteLine();
            writer.WriteLine("Parameters:");
            emptyArgs.GetOptionSet().WriteOptionDescriptions(writer);
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
// This file is part of AnimeRecs.GetMalRecs.
//
// AnimeRecs.GetMalRecs is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.GetMalRecs is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.GetMalRecs.  If not, see <http://www.gnu.org/licenses/>.