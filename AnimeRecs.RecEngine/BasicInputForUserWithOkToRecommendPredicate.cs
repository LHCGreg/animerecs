using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Simple user data containing only a set of item ratings. In addition to checking whether the user has rated an item,
    /// ItemIsOkToRecommend will also check a predicate.
    /// </summary>
    public class BasicInputForUserWithOkToRecommendPredicate : IBasicInputForUser
    {
        public IDictionary<int, float> Ratings { get; private set; }

        private Predicate<int> m_okToRecommendItemFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratings"></param>
        /// <param name="okToRecommendItemFunc">A predicate that takes an item id and returns true if the item is ok to recommend.
        /// It does not need to check if the user has already rated the item. That is already checked.</param>
        public BasicInputForUserWithOkToRecommendPredicate(IDictionary<int, float> ratings, Predicate<int> okToRecommendItemFunc)
        {
            Ratings = ratings;
            m_okToRecommendItemFunc = okToRecommendItemFunc;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Ratings.ContainsKey(itemId) && m_okToRecommendItemFunc(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Ratings.ContainsKey(itemId);
        }
    }
}
