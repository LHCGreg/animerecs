using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using MiscUtil.IO;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.ClientLib.Registrations;
using System.Reflection;

namespace AnimeRecs.RecService.ClientLib
{
    /// <summary>
    /// Client to the rec service. This class is thread-safe.
    /// </summary>
    public class AnimeRecsClient : IDisposable
    {
        public int PortNumber { get; private set; }

        public static readonly int DefaultPort = 5541;

        private ResponseToRecsConverter m_responseToRecs = new ResponseToRecsConverter();

        public AnimeRecsClient()
        {
            PortNumber = DefaultPort;
        }

        public AnimeRecsClient(int portNumber)
        {
            PortNumber = portNumber;
        }

        /// <exception cref="AnimeRecs.RecService.DTO.RecServiceErrorException">The recommendation service returned an error.
        /// Consult the ErrorCode property for more information.</exception>
        public string Ping(string message, int receiveTimeoutInMs = 0)
        {
            Operation<PingRequest> operation = new Operation<PingRequest>(
                opName: OpNames.Ping,
                payload: new PingRequest(message)
            );

            PingResponse pingResponse = DoOperationWithResponseBody<PingResponse>(operation, receiveTimeoutInMs: receiveTimeoutInMs);
            return pingResponse.ResponseMessage;
        }

        public void LoadRecSource(string name, bool replaceExisting, RecSourceParams parameters, int receiveTimeoutInMs = 0)
        {
            Operation<LoadRecSourceRequest<RecSourceParams>> operation = new Operation<LoadRecSourceRequest<RecSourceParams>>(
                opName: OpNames.LoadRecSource,
                payload: new LoadRecSourceRequest<RecSourceParams>(
                    name: name, type: parameters.GetRecSourceTypeName(), replaceExisting: replaceExisting, parameters: parameters
                )
            );

            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public void LoadRecSource(LoadRecSourceRequest request, int receiveTimeoutInMs = 0)
        {
            Operation<LoadRecSourceRequest> operation = new Operation<LoadRecSourceRequest>(
                opName: OpNames.LoadRecSource,
                payload: request
            );
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public void UnloadRecSource(string name, int receiveTimeoutInMs = 0)
        {
            Operation<UnloadRecSourceRequest> operation = new Operation<UnloadRecSourceRequest>(
                opName: OpNames.UnloadRecSource,
                payload: new UnloadRecSourceRequest(name)
            );

            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public string GetRecSourceType(string recSourceName, int receiveTimeoutInMs = 0)
        {
            Operation<GetRecSourceTypeRequest> operation = new Operation<GetRecSourceTypeRequest>(
                opName: OpNames.GetRecSourceType,
                payload: new GetRecSourceTypeRequest(recSourceName)
            );

            GetRecSourceTypeResponse responseBody = DoOperationWithResponseBody<GetRecSourceTypeResponse>(operation, receiveTimeoutInMs);
            return responseBody.RecSourceType;
        }

        public void ReloadTrainingData(ReloadBehavior behavior, bool finalize, int receiveTimeoutInMs = 0)
        {
            Operation<ReloadTrainingDataRequest> operation = new Operation<ReloadTrainingDataRequest>(
                opName: OpNames.ReloadTrainingData,
                payload: new ReloadTrainingDataRequest(behavior, finalize)
            );
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs);
        }

        public void FinalizeRecSources(int receiveTimeoutInMs = 0)
        {
            Operation<FinalizeRecSourcesRequest> operation = new Operation<FinalizeRecSourcesRequest>(
                opName: OpNames.FinalizeRecSources,
                payload: new FinalizeRecSourcesRequest()
            );
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs);
        }

        public MalRecResults<IEnumerable<IRecommendation>> GetMalRecommendations(IDictionary<int, RecEngine.MAL.MalListEntry> animeList,
            string recSourceName, int numRecsDesired, decimal targetScore, int receiveTimeoutInMs = 0)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OpNames.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetScore(recSourceName, numRecsDesired, targetScore, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendations(operation, receiveTimeoutInMs);
        }

        public MalRecResults<IEnumerable<IRecommendation>> GetMalRecommendationsWithPercentileTarget(
           IDictionary<int, RecEngine.MAL.MalListEntry> animeList, string recSourceName, int numRecsDesired, decimal targetPercentile,
           int receiveTimeoutInMs = 0)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OpNames.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetFraction(recSourceName, numRecsDesired, targetPercentile, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendations(operation, receiveTimeoutInMs);
        }

        private List<DTO.MalListEntry> CreateDtoAnimeList(IDictionary<int, RecEngine.MAL.MalListEntry> animeList)
        {
            List<DTO.MalListEntry> dtoAnimeList = new List<DTO.MalListEntry>();
            foreach (int animeId in animeList.Keys)
            {
                RecEngine.MAL.MalListEntry engineEntry = animeList[animeId];
                DTO.MalListEntry dtoEntry = new DTO.MalListEntry(animeId, engineEntry.Rating, engineEntry.Status, engineEntry.NumEpisodesWatched);
                dtoAnimeList.Add(dtoEntry);
            }

            return dtoAnimeList;
        }

        private MalRecResults<IEnumerable<IRecommendation>> GetMalRecommendations(Operation<GetMalRecsRequest> operation, int receiveTimeoutInMs)
        {
            GetMalRecsResponse response = DoOperationWithResponseBody<GetMalRecsResponse>(operation, receiveTimeoutInMs);

            // This should be set as if we were running against an in-process rec source.
            // So it should be an IEnumerable<AverageScoreRecommendation> if getting recs from an AverageScore rec source, etc.
            // MalRecResultsExtensions.cs contains extension methods for "casting" MalRecResults to a strongly-typed MalRecResults.
            IEnumerable<IRecommendation> results;

            Type responseType = response.GetType();
            // check if m_responseToRecs implements IResponseToRecsConverter<responseType>
            Type converterInterfaceOpenType = typeof(IResponseToRecsConverter<>);
            Type converterInterfaceClosedType = converterInterfaceOpenType.MakeGenericType(responseType);
            if (converterInterfaceClosedType.IsInstanceOfType(m_responseToRecs))
            {
                // Good, we have a handler.
                MethodInfo converterMethod = converterInterfaceClosedType.GetMethod(ResponseToRecsConverter.ConvertMethodName);
                object conversionResultObj = converterMethod.Invoke(m_responseToRecs, new object[] { response });
                results = (IEnumerable<IRecommendation>)conversionResultObj;
            }
            else
            {
                // Fallback. This will throw an exception if the recommendation DTO type is registered in the DTO assembly
                // but no handler is registered in this assembly.
                GetMalRecsResponse<DTO.Recommendation> specificResponse = (GetMalRecsResponse<DTO.Recommendation>)response;
                List<IRecommendation> recommendations = new List<IRecommendation>();
                foreach (DTO.Recommendation dtoRec in specificResponse.Recommendations)
                {
                    recommendations.Add(new BasicRecommendation(dtoRec.MalAnimeId));
                }

                results = recommendations;
            }

            Dictionary<int, RecEngine.MAL.MalAnime> animes = new Dictionary<int, RecEngine.MAL.MalAnime>();
            foreach (DTO.MalAnime dtoAnime in response.Animes)
            {
                animes[dtoAnime.MalAnimeId] = new RecEngine.MAL.MalAnime(dtoAnime.MalAnimeId, dtoAnime.MalAnimeType, dtoAnime.Title);
            }

            MalRecResults<IEnumerable<IRecommendation>> resultsWithAnimes = new MalRecResults<IEnumerable<IRecommendation>>(results, animes, response.RecommendationType);

            return resultsWithAnimes;
        }

        private TResponse DoOperation<TResponse>(Operation operation, int receiveTimeoutInMs)
            where TResponse : Response
        {
            string responseJsonString;
            TResponse response = DoOperation<TResponse>(operation, receiveTimeoutInMs, out responseJsonString);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="operation"></param>
        /// <param name="receiveTimeoutInMs"></param>
        /// <returns></returns>
        /// <exception cref="AnimeRecs.RecService.DTO.RecServiceErrorException">The recommendation service returned an error.
        /// Consult the ErrorCode property for more information.</exception>
        private TResponse DoOperation<TResponse>(Operation operation, int receiveTimeoutInMs, out string responseJsonString)
            where TResponse : Response
        {
            string operationJsonString = JsonConvert.SerializeObject(operation);
            byte[] operationJsonBytes = Encoding.UTF8.GetBytes(operationJsonString);

            byte[] responseJsonBytes;

            using (TcpClient socket = CreateTcpClient(receiveTimeoutInMs))
            {
                int length = operationJsonBytes.Length;
                int lengthNetworkOrder = IPAddress.HostToNetworkOrder(length);
                byte[] lengthBytes = BitConverter.GetBytes(lengthNetworkOrder);
                Logging.Log.Trace("Sending bytes.");
                SocketSendAll(socket.Client, lengthBytes);
                SocketSendAll(socket.Client, operationJsonBytes);
                Logging.Log.Trace("Sent bytes.");
                using (NetworkStream socketStream = socket.GetStream())
                {
                    byte[] responseLengthBytes = StreamUtil.ReadExactly(socketStream, 4);
                    int responseLengthNetworkOrder = BitConverter.ToInt32(responseLengthBytes, 0);
                    int responseLength = IPAddress.NetworkToHostOrder(responseLengthNetworkOrder);
                    responseJsonBytes = StreamUtil.ReadExactly(socketStream, responseLength);
                    Logging.Log.Trace("Got response.");
                }
            }

            responseJsonString = Encoding.UTF8.GetString(responseJsonBytes);
            Logging.Log.Trace("Decoded response string.");
            TResponse response = JsonConvert.DeserializeObject<TResponse>(responseJsonString);
            Logging.Log.Trace("Deserialized response.");
            if (response.Error != null)
            {
                throw new RecServiceErrorException(response.Error);
            }
            else
            {
                return response;
            }
        }

        private TResponseBody DoOperationWithResponseBody<TResponseBody>(Operation operation, int receiveTimeoutInMs)
        {
            Response<TResponseBody> response = DoOperation<Response<TResponseBody>>(operation, receiveTimeoutInMs);
            return response.Body;
        }

        private void DoOperationWithoutResponseBody(Operation operation, int receiveTimeoutInMs)
        {
            DoOperation<Response>(operation, receiveTimeoutInMs);
        }

        private TcpClient CreateTcpClient(int receiveTimeoutInMs)
        {
            TcpClient client = new TcpClient();
            Logging.Log.Trace("Connecting to rec service.");
            client.Connect(IPAddress.Loopback, PortNumber);
            client.SendTimeout = 3000;
            client.ReceiveTimeout = receiveTimeoutInMs;
            Logging.Log.Trace("Connected to rec service.");
            return client;
        }

        private void SocketSendAll(Socket socket, byte[] bytes)
        {
            int numSent = 0;
            while (numSent < bytes.Length)
            {
                int numSentThisTime = socket.Send(bytes, offset: numSent, size: bytes.Length - numSent, socketFlags: SocketFlags.None);
                numSent += numSentThisTime;
            }
        }

        public void Dispose()
        {
            ;
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.RecService.ClientLib.
//
// AnimeRecs.RecService.ClientLib is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.ClientLib is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.ClientLib.  If not, see <http://www.gnu.org/licenses/>.