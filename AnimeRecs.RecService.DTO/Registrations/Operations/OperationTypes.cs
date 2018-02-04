using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    public static class OperationTypes
    {
        public static string Ping { get { return "Ping"; } }
        public static string LoadRecSource { get { return "LoadRecSource"; } }
        public static string UnloadRecSource { get { return "UnloadRecSource"; } }
        public static string ReloadTrainingData { get { return "ReloadTrainingData"; } }
        public static string GetMalRecs { get { return "GetMalRecs"; } }
        public static string GetRecSourceType { get { return "GetRecSourceType"; } }
        public static string FinalizeRecSources { get { return "FinalizeRecSources"; } }

        private static IDictionary<string, Func<Operation>> s_operationFactories =
            new Dictionary<string, Func<Operation>>(StringComparer.OrdinalIgnoreCase)
            {
                { Ping, () => new Operation<PingRequest>() { OpName = Ping } },
                { LoadRecSource, () => new Operation<LoadRecSourceRequest>() { OpName = LoadRecSource } },
                { UnloadRecSource, () => new Operation<UnloadRecSourceRequest>() { OpName = UnloadRecSource } },
                { ReloadTrainingData, () => new Operation<ReloadTrainingDataRequest>() { OpName = ReloadTrainingData } },
                { GetMalRecs, () => new Operation<GetMalRecsRequest>() { OpName = GetMalRecs } },
                { GetRecSourceType, () => new Operation<GetRecSourceTypeRequest>() { OpName = GetRecSourceType } },
                { FinalizeRecSources, () => new Operation<FinalizeRecSourcesRequest>() { OpName = FinalizeRecSources } },
            };
        public static IDictionary<string, Func<Operation>> OperationFactories { get { return s_operationFactories; } }
    }
}


