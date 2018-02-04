using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class OperationJsonConverter : JsonCreationConverter<Operation>
    {
        protected override Operation Create(Type objectType, JObject jObject)
        {
            string opName = jObject.Value<string>("OpName");

            if (!OperationTypes.OperationFactories.TryGetValue(opName, out Func<Operation> opFactory))
            {
                throw new Newtonsoft.Json.JsonException(string.Format("Operation {0} not recognized.", opName));
            }

            return opFactory();
        }
    }
}
