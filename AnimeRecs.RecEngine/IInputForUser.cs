using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface IInputForUser
    {
        /// <summary>
        /// Returns true if the item is ok to return as a recommendation. The most obvious filter is to check
        /// if the user has already seen the given item. This is a good place to put domain-specific filters.
        /// For example, not recommending a sequel if the user has not seen the original.
        /// 
        /// Recommendation sources should consult this function for each potential recommendation and honor the result.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool ItemIsOkToRecommend(int itemId);

        /// <summary>
        /// Returns true if the input contains the given item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool ContainsItem(int itemId);
    }
}
