using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class BasicPositiveFeedbackForUserWithOkToRecommendPredicate : IPositiveFeedbackForUser
    {
        public ICollection<int> Items { get; private set; }

        private Predicate<int> m_okToRecommendItemFunc;

        public BasicPositiveFeedbackForUserWithOkToRecommendPredicate(ICollection<int> items, Predicate<int> okToRecommendItemFunc)
        {
            Items = items;
            m_okToRecommendItemFunc = okToRecommendItemFunc;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Items.Contains(itemId) && m_okToRecommendItemFunc(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Items.Contains(itemId);
        }
    }
}
