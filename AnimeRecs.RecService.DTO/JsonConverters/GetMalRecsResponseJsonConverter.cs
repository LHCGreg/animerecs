using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class GetMalRecsResponseJsonConverter : JsonCreationConverter<GetMalRecsResponse>
    {
        protected override GetMalRecsResponse Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            string recType = jObject.Value<string>("RecommendationType");

            if (recType == null || !RecommendationTypes.GetMalRecsResponseFactories.ContainsKey(recType))
            {
                // fallback
                return new GetMalRecsResponse<Recommendation>();
            }

            return RecommendationTypes.GetMalRecsResponseFactories[recType]();
        }
    }
}
