using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Can provide a set of recommendations given input for a user.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TRecommendation"></typeparam>
    public interface IRecommendationSource<in TInput, out TRecommendationResults, out TRecommendation>
        where TInput : IInputForUser
        where TRecommendationResults : IEnumerable<TRecommendation>
        where TRecommendation : IRecommendation
    {
        /// <summary>
        /// Tries to to get <paramref name="numRecommendationsToTryToGet"/> recommendations for the given user.
        /// The recommendation source may return fewer if it is not able to provide that many. The recommendation source should
        /// honor the ItemIsOkToRecommend() method of the input and avoid returning items that are not ok to recommend.
        /// </summary>
        /// <param name="inputForUser"></param>
        /// <param name="numRecommendationsToTryToGet"></param>
        /// <returns></returns>
        TRecommendationResults GetRecommendations(TInput inputForUser, int numRecommendationsToTryToGet);
    }
}
