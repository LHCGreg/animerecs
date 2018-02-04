using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Input classifier for basic input.
    /// </summary>
    public class BasicInputForUserPercentageClassifier : PercentageRatingClassifier<BasicInputForUser>
    {
        public BasicInputForUserPercentageClassifier(double goodPercentage)
            : base(goodPercentage, GetBasicInputForUserSubset)
        {
            ;
        }

        private static BasicInputForUser GetBasicInputForUserSubset(BasicInputForUser user, ICollection<int> itemIds)
        {
            IDictionary<int, float> ratingsSubset = new Dictionary<int, float>();
            foreach(int itemId in itemIds)
            {
                ratingsSubset[itemId] = user.Ratings[itemId];
            }
            return new BasicInputForUser(ratingsSubset);
        }
    }
}
