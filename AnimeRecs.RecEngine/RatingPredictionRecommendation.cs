using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class RatingPredictionRecommendation : IRecommendation
    {
        public int ItemId { get; private set; }
        public double PredictedRating { get; private set; }

        public RatingPredictionRecommendation(int itemId, double predictedRating)
        {
            ItemId = itemId;
            PredictedRating = predictedRating;
        }

        public override string ToString()
        {
            return PredictedRating.ToString();
        }
    }
}
