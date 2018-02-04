using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    public static class RecommendationTypes
    {
        public static string BasicRecommendation { get { return "BasicRecommendation"; } }
        public static string AverageScore { get { return "AverageScore"; } }
        public static string MostPopular { get { return "MostPopular"; } }
        public static string AnimeRecs { get { return "AnimeRecs"; } }
        public static string RatingPrediction { get { return "RatingPrediction"; } }

        // Will fall back to a basic Recomendation is none of these recomendation types match
        private static IDictionary<string, Func<GetMalRecsResponse>> s_getMalRecsResponseFactories =
            new Dictionary<string, Func<GetMalRecsResponse>>(StringComparer.OrdinalIgnoreCase)
            {
                { AverageScore, () => new GetMalRecsResponse<AverageScoreRecommendation>() { RecommendationType = AverageScore } },
                { MostPopular, () => new GetMalRecsResponse<MostPopularRecommendation>() { RecommendationType = MostPopular } },
                { AnimeRecs, () => new GetMalRecsResponse<AnimeRecsRecommendation, MalAnimeRecsExtraResponseData>() { RecommendationType = AnimeRecs } },
                { RatingPrediction, () => new GetMalRecsResponse<RatingPredictionRecommendation>() { RecommendationType = RatingPrediction } }
            };
        public static IDictionary<string, Func<GetMalRecsResponse>> GetMalRecsResponseFactories { get { return s_getMalRecsResponseFactories; } }

    }
}
