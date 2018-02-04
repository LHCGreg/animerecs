using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalPositiveFeedbackRecResults : IEnumerable<RatingPredictionRecommendation>
    {
        public IEnumerable<RatingPredictionRecommendation> Recommendations { get; private set; }
        public decimal TargetScoreUsed { get; private set; }

        public MalPositiveFeedbackRecResults(IEnumerable<RatingPredictionRecommendation> recommendations, decimal targetScoreUsed)
        {
            Recommendations = recommendations;
            TargetScoreUsed = targetScoreUsed;
        }

        public IEnumerator<RatingPredictionRecommendation> GetEnumerator()
        {
            return Recommendations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
