using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Evaluation
{
    /// <summary>
    /// Evalutes the effectiveness of a recommendation source by removing some items from the input for each user and seeing
    /// if the recomendation source recommends an item that was removed.
    /// </summary>
    public class TopNEvaluator
    {
        public TopNEvaluator()
        {
            ;
        }

        private double m_fractionOfInputToSetAsideForEvaluation = .2;
        public double FractionOfInputToSetAsideForEvaluation
        {
            get { return m_fractionOfInputToSetAsideForEvaluation; }
            set { m_fractionOfInputToSetAsideForEvaluation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TRecommendation"></typeparam>
        /// <param name="recSource">A recommendation source that has already been trained.</param>
        /// <param name="users">The users to use in evaluation by getting recommendations for them.</param>
        /// <param name="goodBadClassifier">Classifier to classify input for a user into liked items and unliked items. If an
        /// item was left out of the input passed to the recommendation source and it gets recommended, this determines whether
        /// it gets considered a true positive or a false positive.</param>
        /// <param name="inputDivisionFunc">Takes a user's classified input and a fraction to set aside (.2 for 20%) and returns
        /// a new user input object and the items that were set aside.</param>
        /// <param name="numRecsToTryToGet">Number of recommendations to try to get for each user.</param>
        /// <returns></returns>
        public EvaluationResults Evaluate<TInput, TRecommendation>(
            IRecommendationSource<TInput, IEnumerable<TRecommendation>, TRecommendation> recSource,
            ICollection<TInput> users,
            IUserInputClassifier<TInput> goodBadClassifier,
            Func<ClassifiedUserInput<TInput>, double, ItemsForInputAndEvaluation<TInput>> inputDivisionFunc,
            int numRecsToTryToGet)

            where TInput : IInputForUser
            where TRecommendation : IRecommendation
        {
            EvaluationResults results = new EvaluationResults()
            {
                TotalTruePositives = 0,
                TotalFalsePositives = 0,
                TotalUnknown = 0,
                TotalFalseNegatives = 0,
                TotalPrecision = 0,
                NumPrecision = 0,
                TotalRecall = 0,
                NumRecall = 0
            };

            foreach (TInput user in users)
            {
                SingleUserEvaluationResults userResults = GetSingleUserEvaluationResults(recSource, user, goodBadClassifier,
                    inputDivisionFunc, numRecsToTryToGet);

                results.TotalTruePositives += userResults.TruePositives;
                results.TotalFalsePositives += userResults.FalsePositives;
                results.TotalUnknown += userResults.Unknowns;
                results.TotalFalseNegatives += userResults.FalseNegatives;

                if (userResults.Precision.HasValue)
                {
                    results.TotalPrecision += userResults.Precision.Value;
                    results.NumPrecision++;
                }

                if (userResults.Recall.HasValue)
                {
                    results.TotalRecall += userResults.Recall.Value;
                    results.NumRecall++;
                }
            }

            return results;
        }

        private SingleUserEvaluationResults GetSingleUserEvaluationResults<TInput, TRecommendation>(
            IRecommendationSource<TInput, IEnumerable<TRecommendation>, TRecommendation> recSource,
            TInput user,
            IUserInputClassifier<TInput> goodBadClassifier,
            Func<ClassifiedUserInput<TInput>, double, ItemsForInputAndEvaluation<TInput>> inputDivisionFunc,
            int numRecsToTryToGet)

            where TInput : IInputForUser
            where TRecommendation : IRecommendation
        {
            // Divide into liked and unliked.
            // Set aside a random X% of the liked and a random X% of the unliked
            ClassifiedUserInput<TInput> classified = goodBadClassifier.Classify(user);
            ItemsForInputAndEvaluation<TInput> divided = inputDivisionFunc(classified, FractionOfInputToSetAsideForEvaluation);

            // Get top N recommendations
            // Keep count of hits and false positives

            List<int> recommendedIds = new List<int>();
            int truePositivesForThisUser = 0;
            int falsePositivesForThisUser = 0;
            int unknownsForThisUser = 0;
            foreach (TRecommendation recommendation in recSource.GetRecommendations(divided.ItemsForInput, numRecsToTryToGet))
            {
                recommendedIds.Add(recommendation.ItemId);

                if (divided.LikedItemsForEvaluation.Contains(recommendation.ItemId))
                {
                    truePositivesForThisUser++;
                }
                else if (divided.UnlikedItemsForEvaluation.Contains(recommendation.ItemId))
                {
                    falsePositivesForThisUser++;
                }
                else
                {
                    unknownsForThisUser++;
                }
            }

            int falseNegativesForThisUser = divided.LikedItemsForEvaluation.Count - truePositivesForThisUser;

            SingleUserEvaluationResults results = new SingleUserEvaluationResults()
            {
                TruePositives = truePositivesForThisUser,
                FalsePositives = falsePositivesForThisUser,
                Unknowns = unknownsForThisUser,
                FalseNegatives = falseNegativesForThisUser,
            };

            return results;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.