using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class LoadRecSourceRequestJsonConverter : JsonCreationConverter<LoadRecSourceRequest>
    {
        protected override LoadRecSourceRequest Create(Type objectType, JObject jObject)
        {
 	        string recSourceType = jObject.Value<string>("Type");

            if (!RecSourceTypes.LoadRecSourceRequestFactories.ContainsKey(recSourceType))
            {
                throw new Newtonsoft.Json.JsonException(string.Format("Rec source type {0} not recognized.", recSourceType));
            }

            return RecSourceTypes.LoadRecSourceRequestFactories[recSourceType]();
        }
    }
}
