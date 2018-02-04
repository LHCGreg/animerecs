using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class BasicPositiveFeedbackForUser : IPositiveFeedbackForUser
    {
        public ICollection<int> Items { get; private set; }

        public BasicPositiveFeedbackForUser(ICollection<int> items)
        {
            Items = items;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Items.Contains(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Items.Contains(itemId);
        }
    }
}
