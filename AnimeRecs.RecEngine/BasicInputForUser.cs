using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Simple user data containing only a set of item ratings.
    /// </summary>
    public class BasicInputForUser : IBasicInputForUser
    {
        public IDictionary<int, float> Ratings { get; private set; }

        public BasicInputForUser()
        {
            Ratings = new Dictionary<int, float>();
        }

        public BasicInputForUser(IDictionary<int, float> ratings)
        {
            Ratings = ratings;
        }

        /// <summary>
        /// Returns true if the user has not rated the item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Ratings.ContainsKey(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Ratings.ContainsKey(itemId);
        }
    }
}
