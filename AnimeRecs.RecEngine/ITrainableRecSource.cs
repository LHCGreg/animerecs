using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface ITrainableRecSource<in TTrainingData, in TInput, out TRecommendationResults, out TRecommendation>
        : ITrainable<TTrainingData>, IRecommendationSource<TInput, TRecommendationResults, TRecommendation>
        where TInput : IInputForUser
        where TRecommendationResults : IEnumerable<TRecommendation>
        where TRecommendation : IRecommendation
    {
    }
}
