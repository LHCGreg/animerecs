using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Classifies basic user input into liked and unliked, with empty "Other". The cutoff between liked and unliked is placed
    /// so as to make the liked portion as close as possible to a given percentage of the input.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class PercentageRatingClassifier<TInput> : IUserInputClassifier<TInput>
        where TInput : IBasicInputForUser, new()
    {
        public double GoodPercentage { get; private set; }
        private Func<TInput, ICollection<int>, TInput> m_ratingsTrimmingFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodPercentage"></param>
        /// <param name="ratingsTrimmingFunc">Takes input for a user and a set of item ids and returns input for the user with only those item ids.</param>
        public PercentageRatingClassifier(double goodPercentage, Func<TInput, ICollection<int>, TInput> ratingsTrimmingFunc)
        {
            GoodPercentage = goodPercentage;
            m_ratingsTrimmingFunc = ratingsTrimmingFunc;
        }

        public ClassifiedUserInput<TInput> Classify(TInput allRatings)
        {
            List<int> itemIds = allRatings.Ratings.Keys.ToList();
            PercentageSplit<int> divided = RecUtils.SplitByPercentage(itemIds, GoodPercentage, (x, y) => allRatings.Ratings[x].CompareTo(allRatings.Ratings[y]));

            return new ClassifiedUserInput<TInput>(
                liked: m_ratingsTrimmingFunc(allRatings, divided.UpperPart),
                notLiked: m_ratingsTrimmingFunc(allRatings, divided.LowerPart),
                other: new TInput()
            );
        }
    }
}
