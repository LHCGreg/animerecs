using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.DAL;
using AnimeRecs.MalApi;
using System.Diagnostics;
using System.Configuration;
using MyMediaLite.RatingPrediction;

namespace AnimeRecs.GetMalRecs
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLine = new CommandLineArgs(args);
            if (commandLine.ShowHelp)
            {
                CommandLineArgs.DisplayHelp(Console.Out);
                return;
            }

            ITrainableRecSource<MalTrainingData, MalUserListEntries, IRecommendation> recSource;
            if (commandLine.RecommenderType == RecommenderType.AverageRating)
            {
                recSource = new MalAverageScoreRecSource(commandLine.MinEpisodesToCountIncomplete, commandLine.UseDropped, commandLine.MinUsersToCountAnime);
            }
            else if (commandLine.RecommenderType == RecommenderType.MostPopular)
            {
                recSource = new MalMostPopularRecSource(commandLine.MinEpisodesToCountIncomplete, commandLine.UseDropped);
            }
            else if (commandLine.RecommenderType == RecommenderType.BiasedMatrixFactorization)
            {
                recSource = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>(
                    new BiasedMatrixFactorization() { BoldDriver = true, FrequencyRegularization = true },
                    commandLine.MinEpisodesToCountIncomplete, commandLine.UseDropped);
            }
            else if (commandLine.RecommenderType == RecommenderType.AnimeRecs)
            {
                recSource = new MalAnimeRecsRecSource(numRecommendersToUse: commandLine.NumRecommenders,
                    fractionConsideredRecommended: commandLine.FractionRecommended, targetFraction: commandLine.TargetFraction,
                    minEpisodesToClassifyIncomplete: commandLine.MinEpisodesToCountIncomplete);
            }
            else
            {
                throw new Exception("oops, missed a recommender type.");
            }

            Stopwatch watch = Stopwatch.StartNew();

            
            MalUserLookupResults animeList;
            using (MyAnimeListApi api = new MyAnimeListApi())
            {
                animeList = api.GetAnimeListForUser(commandLine.MalUser);
            }
            watch.Stop();
            Console.WriteLine("Looking up user: {0}", watch.Elapsed);

            watch.Restart();
            string postgresConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;

            MalTrainingData rawData;
            using (PgMalDataLoader loader = new PgMalDataLoader(postgresConnectionString))
            {
                rawData = loader.LoadRawData();
            }
            watch.Stop();
            Console.WriteLine("Loading data: {0}", watch.Elapsed);

            MalUserListEntries inputForUser = new MalUserListEntries(animeList, rawData.Animes);

            watch.Restart();
            recSource.Train(rawData);
            watch.Stop();
            Console.WriteLine("Training: {0}", watch.Elapsed);

            watch.Restart();
            IEnumerable<IRecommendation> recs = recSource.GetRecommendations(inputForUser, commandLine.NumRecs);
            watch.Stop();
            Console.WriteLine("Calculating recs: {0}", watch.Elapsed);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(recSource);
            int recNum = 1;
            foreach (IRecommendation rec in recs)
            {
                int animeId = rec.ItemId;
                string title = rawData.Animes[animeId].Title;
                Console.WriteLine("{0}.\t{1} {2}", recNum, title, rec);
                recNum++;
            }
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