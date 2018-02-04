using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    public static class RecSourceTypes
    {
        public const string AverageScore = "AverageScore";
        public const string MostPopular = "MostPopular";
        public const string AnimeRecs = "AnimeRecs";
        public const string BiasedMatrixFactorization = "BiasedMatrixFactorization";
        public const string SVDPlusPlus = "SVDPlusPlus";
        public const string ItemKNN = "ItemKNN";
        public const string BPRMF = "BPRMF";

        private static IDictionary<string, Func<LoadRecSourceRequest>> s_LoadRecSourceRequestFactories =
            new Dictionary<string, Func<LoadRecSourceRequest>>(StringComparer.OrdinalIgnoreCase)
            {
                { AverageScore, () => new LoadRecSourceRequest<AverageScoreRecSourceParams>() { Type = AverageScore } },
                { MostPopular, () => new LoadRecSourceRequest<MostPopularRecSourceParams>() { Type = MostPopular } },
                { AnimeRecs, () => new LoadRecSourceRequest<AnimeRecsRecSourceParams>() { Type = AnimeRecs } },
                { BiasedMatrixFactorization, () => new LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams>() { Type = BiasedMatrixFactorization } },
                { SVDPlusPlus, () => new LoadRecSourceRequest<SVDPlusPlusRecSourceParams>() { Type = SVDPlusPlus } },
                { ItemKNN, () => new LoadRecSourceRequest<ItemKNNRecSourceParams>() { Type = ItemKNN } },
                { BPRMF, () => new LoadRecSourceRequest<BPRMFRecSourceParams>() { Type = BPRMF } }
            };
        public static IDictionary<string, Func<LoadRecSourceRequest>> LoadRecSourceRequestFactories { get { return s_LoadRecSourceRequestFactories; } }
    }
}
