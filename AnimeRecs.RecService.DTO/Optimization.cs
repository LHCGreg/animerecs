using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace AnimeRecs.RecService.DTO
{
    public static class Optimization
    {
        /// <summary>
        /// Serializes/Deserializes all JSON DTO types, to get ensure the JSON.NET assembly is loaded
        /// and to get JSON.NET to cache its reflection stuff. Doing this can take up to 200ms off
        /// the first request.
        /// </summary>
        public static void WarmUpJsonSerializing()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Type[] types = thisAssembly.GetTypes();
            Type[] emptyTypeArray = new Type[0];
            object[] emptyObjectArray = new object[0];

            // Serialize/Deserialize all [JsonClass]
            List<Type> simpleClassesToPreload = types.Where(type => type.IsDefined(typeof(JsonClassAttribute), inherit: false)).ToList();
            foreach (Type type in simpleClassesToPreload)
            {
                object instance = type.GetConstructor(emptyTypeArray).Invoke(emptyObjectArray);
                string serialized = JsonConvert.SerializeObject(instance);
                object deserialized = JsonConvert.DeserializeObject(serialized, type);
            }

            // Serialize/Deserialize all Response<T> where T has [ResponseJsonClass]
            List<Type> responseDataTypes = types.Where(type => type.IsDefined(typeof(ResponseJsonClassAttribute), inherit: false)).ToList();
            Type openResponseType = typeof(Response<>);
            Type[] typeArgs = new Type[1];
            foreach (Type responseDataType in responseDataTypes)
            {
                typeArgs[0] = responseDataType;
                Type closedResponseType = openResponseType.MakeGenericType(typeArgs);

                object instance = closedResponseType.GetConstructor(emptyTypeArray).Invoke(emptyObjectArray);
                string serialized = JsonConvert.SerializeObject(instance);
                object deserialized = JsonConvert.DeserializeObject(serialized, closedResponseType);
            }

            // Serialize/Deserialize all GetMalRecsResponse<T> types
            List<GetMalRecsResponse> getMalRecsResponseInstances = RecommendationTypes.GetMalRecsResponseFactories.Values.Select(
                getMalRecsResponseInstanceFactory => getMalRecsResponseInstanceFactory()).ToList();

            getMalRecsResponseInstances.Add(new GetMalRecsResponse<Recommendation>());

            foreach (GetMalRecsResponse responseDataInstance in getMalRecsResponseInstances)
            {
                string serialized = JsonConvert.SerializeObject(responseDataInstance);
                object deserialized = JsonConvert.DeserializeObject<GetMalRecsResponse>(serialized);
            }

            // Serialize/Deserialize all Response<T> where T is GetMalRecsResponse<U>

            foreach (GetMalRecsResponse responseDataInstance in getMalRecsResponseInstances)
            {
                typeArgs[0] = responseDataInstance.GetType();
                Type closedResponseType = openResponseType.MakeGenericType(typeArgs);

                object instance = closedResponseType.GetConstructor(emptyTypeArray).Invoke(emptyObjectArray);
                PropertyInfo bodyProperty = closedResponseType.GetProperty(nameof(Response<GetMalRecsResponse>.Body));
                bodyProperty.SetMethod.Invoke(instance, new object[] { responseDataInstance });

                string serialized = JsonConvert.SerializeObject(instance);
                object deserialized = JsonConvert.DeserializeObject<Response<GetMalRecsResponse>>(serialized);
            }

            // Serialize/Deserialize all Operation<T> types
            List<Operation> operationInstances = OperationTypes.OperationFactories.Values.Select(
                operationInstanceFactory => operationInstanceFactory()).ToList();

            foreach (Operation operationInstance in operationInstances)
            {
                string serialized = JsonConvert.SerializeObject(operationInstance);

                if (!(operationInstance is Operation<LoadRecSourceRequest>))
                {
                    object deserialized = JsonConvert.DeserializeObject<Operation>(serialized);
                }
            }

            // Serialize/Deserialize all LoadRecSourceRequest<T>

            List<LoadRecSourceRequest> loadRecSourceRequestInstances = RecSourceTypes.LoadRecSourceRequestFactories.Values.Select(
                loadRecSourceRequestDataFactory => loadRecSourceRequestDataFactory()).ToList();

            foreach (LoadRecSourceRequest instance in loadRecSourceRequestInstances)
            {
                string serialized = JsonConvert.SerializeObject(instance);
                object deserialized = JsonConvert.DeserializeObject<LoadRecSourceRequest>(serialized);
            }
        }
    }
}
