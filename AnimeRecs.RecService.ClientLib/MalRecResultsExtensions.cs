using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.ClientLib
{
    public static class MalRecResultsExtensions
    {
        public static MalRecResults<T> CastRecs<T>(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
            where T : IEnumerable<IRecommendation>
        {
            return new MalRecResults<T>((T)basicResults.Results, basicResults.AnimeInfo, basicResults.RecommendationType);
        }
        
        public static MalRecResults<IEnumerable<RecEngine.AverageScoreRecommendation>> AsAverageScoreResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<RecEngine.AverageScoreRecommendation> results = (IEnumerable<RecEngine.AverageScoreRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.AverageScoreRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<RecEngine.MAL.MalAnimeRecsResults> AsAnimeRecsResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
            {
                RecEngine.MAL.MalAnimeRecsResults results = (RecEngine.MAL.MalAnimeRecsResults)basicResults.Results;
                return new MalRecResults<RecEngine.MAL.MalAnimeRecsResults>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation>> AsMostPopularResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation> results = (IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.MostPopularRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation>> AsRatingPredictionResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.RatingPrediction, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation> results = (IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.RatingPredictionRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }
    }
}
