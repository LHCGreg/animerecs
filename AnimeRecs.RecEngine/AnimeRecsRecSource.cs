using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.Utils;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Recommendation source using the algorithm used by www.animerecs.com. 
    /// </summary>
    /// <typeparam name="TUnderlyingTrainingData"></typeparam>
    /// <typeparam name="TTrainingDataUserRatings"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <remarks>Takes N users as input. These users are called "recommenders". The recommenders' ratings are divided into liked and
    /// unliked items. The liked items are considered to be recommendations.
    /// 
    /// To get recommendations for a user, go through all recommenders and calculate
    /// (NumRecommendationsLiked / (NumRecommendationsLiked + NumRecommendationsNotLiked)).
    /// That is called the user's compatibility rating with that recommender.
    /// Calculate a 95% confidence interval of the compatibility rating.
    /// Sort recommenders by the low endpoint of the confidence interval. This has the effect of giving more weight to recommendations
    /// that are more certain.
    /// Go through the sorted list of recommenders and recommend each recommendation in turn. The caller may supply a function
    /// for ordering the recommendations by a particular user. One way of ordering might be to order by the recommender's rating
    /// then by the item's average rating.
    /// </remarks>
    public class AnimeRecsRecSource<TUnderlyingTrainingData, TTrainingDataUserRatings, TInput>
        : ITrainableRecSource<AnimeRecsTrainingData<TUnderlyingTrainingData, TTrainingDataUserRatings>, AnimeRecsInput<TInput>,
        AnimeRecsResults, AnimeRecsRecommendation>

        where TUnderlyingTrainingData : IBasicTrainingData<TTrainingDataUserRatings>
        where TTrainingDataUserRatings : IInputForUserWithItemIds
        where TInput : IInputForUser
    {
        const int DefaultNumRecommenders = 100;
        private int m_numRecommenders = DefaultNumRecommenders;
        public int NumRecommenders { get { return m_numRecommenders; } set { m_numRecommenders = value; } }

        private List<Recommender> m_recommenders = new List<Recommender>();
        private List<Recommender> Recommenders { get { return m_recommenders; } set { m_recommenders = value; } }

        public AnimeRecsRecSource(int numRecommenders = DefaultNumRecommenders)
        {
            NumRecommenders = numRecommenders;
        }

        public void Train(AnimeRecsTrainingData<TUnderlyingTrainingData, TTrainingDataUserRatings> trainingData)
        {
            Recommenders = new List<Recommender>();
            // Use first N users as recommenders
            foreach (int userId in trainingData.TrainingData.Users.Keys)
            {
                AddRecommender(userId, trainingData.TrainingData.Users[userId], trainingData.RecommenderRatingClassifier);

                if (m_recommenders.Count >= NumRecommenders)
                {
                    break;
                }
            }
        }

        private void AddRecommender<TUserRatings>(int userId, TUserRatings ratings, IUserInputClassifier<TUserRatings> recommenderRatingClassifier)
            where TUserRatings : IInputForUserWithItemIds
        {
            ClassifiedUserInput<TUserRatings> classifiedRatings = recommenderRatingClassifier.Classify(ratings);

            List<int> recs = new List<int>(classifiedRatings.Liked.ItemIds);
            Recommender recommender = new Recommender(userId, recs);
            Recommenders.Add(recommender);
        }

        public AnimeRecsResults GetRecommendations(AnimeRecsInput<TInput> input,
            int numRecommendationsToTryToGet)
        {
            // Sort recommenders by the low endpoint of the compatibility confidence interval
            List<AnimeRecsRecommenderUser> recommendersWithCompatibility = new List<AnimeRecsRecommenderUser>();
            foreach (Recommender recommender in Recommenders)
            {
                ICollection<int> recsLiked = new HashSet<int>();
                ICollection<int> recsNotLiked = new HashSet<int>();
                ICollection<int> recsInconclusive = new HashSet<int>();
                ICollection<int> recsNotInCommon = new HashSet<int>();
                foreach (int recItemId in recommender.Recommendations)
                {
                    if (input.ClassifiedInput.Liked.ContainsItem(recItemId))
                    {
                        recsLiked.Add(recItemId);
                    }
                    else if (input.ClassifiedInput.NotLiked.ContainsItem(recItemId))
                    {
                        recsNotLiked.Add(recItemId);
                    }
                    else if (input.OriginalInput.ContainsItem(recItemId))
                    {
                        recsInconclusive.Add(recItemId);
                    }
                    else
                    {
                        recsNotInCommon.Add(recItemId);
                    }
                }

                recommendersWithCompatibility.Add(new AnimeRecsRecommenderUser(
                    userId: recommender.UserId,
                    recsLiked: recsLiked,
                    recsNotLiked: recsNotLiked,
                    recsInconclusive: recsInconclusive,
                    recsNotInCommon: recsNotInCommon
                    ));
            }

            recommendersWithCompatibility.Sort((x, y) => y.CompatibilityLowEndpoint.GetValueOrDefault().CompareTo(x.CompatibilityLowEndpoint.GetValueOrDefault()));

            List<AnimeRecsRecommendation> recs = new List<AnimeRecsRecommendation>();
            HashSet<int> recIds = new HashSet<int>();

            foreach (AnimeRecsRecommenderUser recommender in recommendersWithCompatibility)
            {
                Recommender staticRecommender = Recommenders.Where(r => r.UserId == recommender.UserId).First();

                IEnumerable<int> itemIdsRecommendedByThisUser = null;
                if (input.OrderingGivenRecommenderAndItemIdsComparable == null)
                {
                    itemIdsRecommendedByThisUser = staticRecommender.Recommendations;
                }
                else
                {
                    itemIdsRecommendedByThisUser = staticRecommender.Recommendations.OrderByDescending((itemId) => new Tuple<int, int>(recommender.UserId, itemId), input.OrderingGivenRecommenderAndItemIdsComparable).ToList();
                }

                foreach (int rec in itemIdsRecommendedByThisUser)
                {
                    if (!recIds.Contains(rec) && input.ItemIsOkToRecommend(rec))
                    {
                        recs.Add(new AnimeRecsRecommendation(recommenderUserId: recommender.UserId, itemId: rec));
                        recIds.Add(rec);
                        if (recs.Count >= numRecommendationsToTryToGet)
                            break;
                    }
                }

                if (recs.Count >= numRecommendationsToTryToGet)
                    break;
            }

            return new AnimeRecsResults(recs, recommendersWithCompatibility);
        }

        private class Recommender
        {
            public int UserId { get; private set; }
            public IList<int> Recommendations { get; private set; }

            public Recommender(int userId, IList<int> recommendations)
            {
                UserId = userId;
                Recommendations = recommendations;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1} recommendations", UserId, Recommendations.Count);
            }
        }
    }

    public class AnimeRecsInput<TUnderlyingInput> : IInputForUser
        where TUnderlyingInput : IInputForUser
    {
        public TUnderlyingInput OriginalInput { get; private set; }
        public ClassifiedUserInput<TUnderlyingInput> ClassifiedInput { get; private set; }
        public Comparison<Tuple<int, int>> OrderingGivenRecommenderAndItemIds { get; private set; }
        internal IComparer<Tuple<int, int>> OrderingGivenRecommenderAndItemIdsComparable { get; private set; }

        public AnimeRecsInput(TUnderlyingInput originalInput, ClassifiedUserInput<TUnderlyingInput> classifiedInput,
            Comparison<Tuple<int, int>> orderingGivenRecommenderAndItemIds)
        {
            OriginalInput = originalInput;
            ClassifiedInput = classifiedInput;
            OrderingGivenRecommenderAndItemIds = orderingGivenRecommenderAndItemIds;
            if (orderingGivenRecommenderAndItemIds != null)
            {
                OrderingGivenRecommenderAndItemIdsComparable = new DelegateComparer<Tuple<int, int>>(orderingGivenRecommenderAndItemIds);
            }
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return OriginalInput.ItemIsOkToRecommend(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return OriginalInput.ContainsItem(itemId);
        }
    }

    public class AnimeRecsRecommendation : IRecommendation
    {
        public int RecommenderUserId { get; private set; }
        public int ItemId { get; private set; }

        public AnimeRecsRecommendation(int recommenderUserId, int itemId)
        {
            RecommenderUserId = recommenderUserId;
            ItemId = itemId;
        }

        public override string ToString()
        {
            return string.Format("User={0} Item={1}", RecommenderUserId, ItemId);
        }
    }

    public class AnimeRecsResults : IEnumerable<AnimeRecsRecommendation>
    {
        /// <summary>
        /// Contains no more than the amount of recommendations asked for.
        /// </summary>
        public IList<AnimeRecsRecommendation> Recommendations { get; private set; }

        /// <summary>
        /// Contains all recommenders sorted by compatibility low endpoint in descending order.
        /// </summary>
        public IList<AnimeRecsRecommenderUser> Recommenders { get; private set; }

        public AnimeRecsResults(IList<AnimeRecsRecommendation> recommendations, IList<AnimeRecsRecommenderUser> recommenders)
        {
            Recommendations = recommendations;
            Recommenders = recommenders;
        }

        public IEnumerator<AnimeRecsRecommendation> GetEnumerator()
        {
            return Recommendations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Recommendations).GetEnumerator();
        }
    }

    public class AnimeRecsRecommenderUser
    {
        public int UserId { get; private set; }
        public ICollection<int> RecsLiked { get; private set; }
        public ICollection<int> RecsNotLiked { get; private set; }
        public ICollection<int> RecsInconclusive { get; private set; }
        public ICollection<int> RecsNotInCommon { get; private set; }
        public int NumRecsWithJudgment { get { return RecsLiked.Count + RecsNotLiked.Count; } }
        public double? Compatibility { get { return NumRecsWithJudgment > 0 ? ((double)RecsLiked.Count) / NumRecsWithJudgment : (double?)null; } }
        public double? CompatibilityLowEndpoint { get; private set; }
        public double? CompatibilityHighEndpoint { get; private set; }
        public ICollection<int> AllRecommendations { get; private set; }

        public AnimeRecsRecommenderUser(int userId, ICollection<int> recsLiked, ICollection<int> recsNotLiked, ICollection<int> recsInconclusive, ICollection<int> recsNotInCommon)
        {
            UserId = userId;
            RecsLiked = recsLiked;
            RecsNotLiked = recsNotLiked;
            RecsInconclusive = recsInconclusive;
            RecsNotInCommon = recsNotInCommon;
            AllRecommendations = new HashSet<int>(RecsLiked.Concat(RecsNotLiked).Concat(RecsInconclusive).Concat(RecsNotInCommon));

            if (NumRecsWithJudgment == 0)
            {
                CompatibilityLowEndpoint = null;
                CompatibilityHighEndpoint = null;
            }
            else
            {
                Tuple<double, double> confidenceInterval95Percent =
                    ConfidenceInterval.Get95PercentConfidenceInterval(Compatibility.Value, NumRecsWithJudgment);
                CompatibilityLowEndpoint = confidenceInterval95Percent.Item1;
                CompatibilityHighEndpoint = confidenceInterval95Percent.Item2;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}/{1} {2:P2} ({3:P2}-{4:P2})", RecsLiked.Count, NumRecsWithJudgment,
                Compatibility, CompatibilityLowEndpoint, CompatibilityHighEndpoint);
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