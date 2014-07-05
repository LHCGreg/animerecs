using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecEngine.Evaluation;
using AnimeRecs.DAL;
using MyMediaLite;
using MyMediaLite.RatingPrediction;
using MyMediaLite.ItemRecommendation;

namespace AnimeRecs.RecEngine.MalEvaluationRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            TopNEvaluator evaluator = new TopNEvaluator();

            var recommendersUnderTest = new List<ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<IRecommendation>, IRecommendation>>();
            List<List<EvaluationResults>> resultsForEachRecommender = new List<List<EvaluationResults>>();

            const int minEpisodesToCountIncomplete = 26;
            const double targetPercentile = 0.25;

            //var averageScoreRecSourceWithoutDropped = new MalAverageScoreRecSource(minEpisodesToCountIncomplete, useDropped: false, minUsersToCountAnime: 50);
            //var mostPopularRecSourceWithoutDropped = new MalMostPopularRecSource(minEpisodesToCountIncomplete, useDropped: false);
            //var defaultBiasedMatrixFactorizationRecSource = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>
            //    (new BiasedMatrixFactorization(), minEpisodesToCountIncomplete, useDropped: true, minUsersToCountAnime: 50);
            //var biasedMatrixFactorizationRecSourceWithBoldDriver = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>
            //    (new BiasedMatrixFactorization() { BoldDriver = true }, minEpisodesToCountIncomplete, useDropped: true, minUsersToCountAnime: 50);
            //var biasedMatrixFactorizationRecSourceWithFactors = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>
            //    (new BiasedMatrixFactorization() { BoldDriver = true, FrequencyRegularization = true, NumFactors = 50 },
            //    minEpisodesToCountIncomplete, useDropped: true, minUsersToCountAnime: 50);
            //var biasedMatrixFactorizationRecSourceWithFactorsAndIters = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>
            //    (new BiasedMatrixFactorization() { BoldDriver = true, FrequencyRegularization = true, NumFactors = 50, NumIter = 50 },
            //    minEpisodesToCountIncomplete, useDropped: true, minUsersToCountAnime: 50);
            //var defaultMatrixFactorizationRecSource = new MalMyMediaLiteRatingPredictionRecSource<MatrixFactorization>
            //    (new MatrixFactorization(), minEpisodesToCountIncomplete, useDropped: true, minUsersToCountAnime: 50);
            //var animeRecsRecSource35 = new MalAnimeRecsRecSourceWithConstantPercentTarget(
            //    numRecommendersToUse: 100,
            //    fractionConsideredRecommended: 0.35,
            //    targetFraction: 0.35,
            //    minEpisodesToClassifyIncomplete: minEpisodesToCountIncomplete
            //);
            //var animeRecsRecSource25 = new MalAnimeRecsRecSourceWithConstantPercentTarget(
            //    numRecommendersToUse: 100,
            //    fractionConsideredRecommended: 0.25,
            //    targetFraction: 0.25,
            //    minEpisodesToClassifyIncomplete: minEpisodesToCountIncomplete
            //);

            var bprmfRecSource = new MalMyMediaLiteItemRecommenderRecSourceWithConstantPercentTarget<BPRMF>(
                new BPRMF() { BiasReg = .01f },
                fractionConsideredRecommended: 0.25,
                minEpisodesToClassifyIncomplete: minEpisodesToCountIncomplete,
                minUsersToCountAnime: 30,
                targetFraction: 0.25
            );

            //recommendersUnderTest.Add(averageScoreRecSourceWithoutDropped);
            //recommendersUnderTest.Add(mostPopularRecSourceWithoutDropped);
            //recommendersUnderTest.Add(defaultBiasedMatrixFactorizationRecSource);
            //recommendersUnderTest.Add(biasedMatrixFactorizationRecSourceWithBoldDriver);
            //recommendersUnderTest.Add(biasedMatrixFactorizationRecSourceWithFactors);
            //recommendersUnderTest.Add(biasedMatrixFactorizationRecSourceWithFactorsAndIters);
            //recommendersUnderTest.Add(defaultMatrixFactorizationRecSource);
            //recommendersUnderTest.Add(animeRecsRecSource35);
            //recommendersUnderTest.Add(animeRecsRecSource25);
            recommendersUnderTest.Add(bprmfRecSource);

            for (int i = 0; i < recommendersUnderTest.Count; i++)
            {
                resultsForEachRecommender.Add(new List<EvaluationResults>());
            }

            IUserInputClassifier<MalUserListEntries> targetClassifier = new MalPercentageRatingClassifier(targetPercentile, minEpisodesToCountIncomplete);

            MalTrainingData rawData;

            string postgresConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
            using (PgMalDataLoader loader = new PgMalDataLoader(postgresConnectionString))
            {
                rawData = loader.LoadMalTrainingData();
            }

            const int numEvaluations = 5;
            const int numRecsToGet = 25;

            for (int pass = 0; pass < numEvaluations; pass++)
            {
                for (int recSourceIndex = 0; recSourceIndex < recommendersUnderTest.Count; recSourceIndex++)
                {
                    ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<IRecommendation>, IRecommendation> recSource = recommendersUnderTest[recSourceIndex];
                    
                    Tuple<MalTrainingData, ICollection<MalUserListEntries>> dataForTrainingAndEvaluation = GetDataForTrainingAndEvaluation(rawData);
                    MalTrainingData trainingData = dataForTrainingAndEvaluation.Item1;
                    ICollection<MalUserListEntries> dataForEvaluation = dataForTrainingAndEvaluation.Item2;

                    recSource.Train(trainingData);

                    EvaluationResults results = evaluator.Evaluate(
                    recSource: recSource,
                    users: dataForEvaluation,
                    goodBadClassifier: targetClassifier,
                    inputDivisionFunc: MalUserListEntries.DivideClassifiedForInputAndEvaluation,
                    numRecsToTryToGet: numRecsToGet
                    );

                    resultsForEachRecommender[recSourceIndex].Add(results);
                }
            }

            for(int recSourceIndex = 0; recSourceIndex < recommendersUnderTest.Count; recSourceIndex++)
            {
                ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<IRecommendation>, IRecommendation> recSource = recommendersUnderTest[recSourceIndex];
                Console.WriteLine(recSource);
                foreach (EvaluationResults resultsForPass in resultsForEachRecommender[recSourceIndex])
                {
                    Console.WriteLine("Precision: {0:P2}\tRecall: {1:P2}",
                        resultsForPass.Precision, resultsForPass.Recall);
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static Tuple<MalTrainingData, ICollection<MalUserListEntries>> GetDataForTrainingAndEvaluation(MalTrainingData rawData)
        {
            List<int> userIds = rawData.Users.Keys.ToList();
            userIds.Shuffle();

            Dictionary<int, MalUserListEntries> trainingUsers = new Dictionary<int, MalUserListEntries>();
            List<MalUserListEntries> evaluationUsers = new List<MalUserListEntries>();

            int numUsersForTraining = userIds.Count / 2;
            for (int i = 0; i < numUsersForTraining; i++)
            {
                trainingUsers[userIds[i]] = rawData.Users[userIds[i]];
            }
            for (int i = numUsersForTraining; i < userIds.Count; i++)
            {
                evaluationUsers.Add(rawData.Users[userIds[i]]);
            }

            MalTrainingData trainingData = new MalTrainingData(trainingUsers, rawData.Animes);

            return new Tuple<MalTrainingData, ICollection<MalUserListEntries>>(trainingData, evaluationUsers);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.MalEvaluationRunner.
//
// AnimeRecs.MalEvaluationRunner is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.MalEvaluationRunner is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.MalEvaluationRunner.  If not, see <http://www.gnu.org/licenses/>.