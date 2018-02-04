using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Non-personalized recommendation source that recommends the most popular items. Each time that an item is in the training data counts
    /// towards its popularity.
    /// </summary>
    /// <typeparam name="TTrainingData"></typeparam>
    /// <typeparam name="TTrainingUserInput"></typeparam>
    public class MostPopularRecSource<TTrainingData, TTrainingUserInput>
        : ITrainableRecSource<TTrainingData, IInputForUser, IEnumerable<MostPopularRecommendation>, MostPopularRecommendation>

        where TTrainingData : IBasicTrainingData<TTrainingUserInput>
        where TTrainingUserInput : IBasicInputForUser
    {
        private List<Tuple<int, int>> m_numRatingsForEachItem = new List<Tuple<int, int>>();

        public void Train(TTrainingData trainingData)
        {
            Dictionary<int, int> numRatingsByItem = new Dictionary<int, int>();
            foreach (int userId in trainingData.Users.Keys)
            {
                foreach (KeyValuePair<int, float> itemIdRatingPair in trainingData.Users[userId].Ratings)
                {
                    int itemId = itemIdRatingPair.Key;
                    if (!numRatingsByItem.ContainsKey(itemId))
                    {
                        numRatingsByItem[itemId] = 0;
                    }

                    numRatingsByItem[itemId]++;
                }
            }

            m_numRatingsForEachItem = numRatingsByItem.Keys
                .OrderByDescending(itemId => numRatingsByItem[itemId])
                .Select(itemId => new Tuple<int, int>(itemId, numRatingsByItem[itemId]))
                .ToList();
        }

        public IEnumerable<MostPopularRecommendation> GetRecommendations(IInputForUser userRatings, int numRecommendationsToTryToGet)
        {
            List<MostPopularRecommendation> recs = new List<MostPopularRecommendation>();
            for (int i = 0; recs.Count < numRecommendationsToTryToGet && i < m_numRatingsForEachItem.Count; i++)
            {
                if (userRatings.ItemIsOkToRecommend(m_numRatingsForEachItem[i].Item1))
                {
                    recs.Add(new MostPopularRecommendation(m_numRatingsForEachItem[i].Item1, popularityRank: i + 1,
                        numRatings: m_numRatingsForEachItem[i].Item2));
                }
            }
            return recs;
        }
    }

    public class MostPopularRecommendation : IRecommendation
    {
        public int ItemId { get; private set; }
        public int PopularityRank { get; private set; }
        public int NumRatings { get; private set; }

        public MostPopularRecommendation(int itemId, int popularityRank, int numRatings)
        {
            ItemId = itemId;
            PopularityRank = popularityRank;
            NumRatings = numRatings;
        }

        public override string ToString()
        {
            return NumRatings.ToString();
        }
    }
}
