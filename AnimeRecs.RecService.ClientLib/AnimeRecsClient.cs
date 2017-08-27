using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using AnimeRecs.RecEngine;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.ClientLib.Registrations;
using AnimeRecs.Utils;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Nito.AsyncEx;

namespace AnimeRecs.RecService.ClientLib
{
    /// <summary>
    /// Client to the rec service. This class is thread-safe.
    /// </summary>
    public class AnimeRecsClient : IDisposable
    {
        public IPAddress AnimeRecsServiceAddress { get; private set; }

        public int PortNumber { get; private set; }

        public static readonly int DefaultPort = 5541;

        private ResponseToRecsConverter m_responseToRecs = new ResponseToRecsConverter();

        public AnimeRecsClient()
            : this(DefaultPort)
        {

        }

        public AnimeRecsClient(int portNumber)
        {
            PortNumber = portNumber;
            AnimeRecsServiceAddress = IPAddress.Loopback;
        }

        /// <exception cref="AnimeRecs.RecService.DTO.RecServiceErrorException">The recommendation service returned an error.
        /// Consult the ErrorCode property for more information.</exception>
        public async Task<string> PingAsync(string message, TimeSpan timeout, CancellationToken cancellationToken)
        {
            // TODO: rethrow non-rec service errors with more information
            Operation<PingRequest> operation = new Operation<PingRequest>(
                opName: OperationTypes.Ping,
                payload: new PingRequest(message)
            );

            PingResponse pingResponse = await DoOperationWithResponseBodyAsync<PingResponse>(operation, timeout,
                cancellationToken, descriptionForErrors: "pinging rec service").ConfigureAwait(false);
            return pingResponse.ResponseMessage;
        }

        public Task LoadRecSourceAsync(string name, bool replaceExisting, RecSourceParams parameters, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<LoadRecSourceRequest<RecSourceParams>> operation = new Operation<LoadRecSourceRequest<RecSourceParams>>(
                opName: OperationTypes.LoadRecSource,
                payload: new LoadRecSourceRequest<RecSourceParams>(
                    name: name, type: parameters.GetRecSourceTypeName(), replaceExisting: replaceExisting, parameters: parameters
                )
            );

            return DoOperationWithoutResponseBodyAsync(operation, timeout, cancellationToken, descriptionForErrors: "loading rec source " + name);
        }

        public Task LoadRecSourceAsync(LoadRecSourceRequest request, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<LoadRecSourceRequest> operation = new Operation<LoadRecSourceRequest>(
                opName: OperationTypes.LoadRecSource,
                payload: request
            );

            return DoOperationWithoutResponseBodyAsync(operation, timeout, cancellationToken, descriptionForErrors: "loading rec source " + request.Name);
        }

        public Task UnloadRecSourceAsync(string name, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<UnloadRecSourceRequest> operation = new Operation<UnloadRecSourceRequest>(
                opName: OperationTypes.UnloadRecSource,
                payload: new UnloadRecSourceRequest(name)
            );

            return DoOperationWithoutResponseBodyAsync(operation, timeout, cancellationToken, descriptionForErrors: "unloading rec source " + name);
        }

        public async Task<string> GetRecSourceTypeAsync(string recSourceName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<GetRecSourceTypeRequest> operation = new Operation<GetRecSourceTypeRequest>(
                opName: OperationTypes.GetRecSourceType,
                payload: new GetRecSourceTypeRequest(recSourceName)
            );

            GetRecSourceTypeResponse responseBody = await DoOperationWithResponseBodyAsync<GetRecSourceTypeResponse>(
                operation, timeout, cancellationToken, descriptionForErrors: "getting type of rec source " + recSourceName).ConfigureAwait(false);
            return responseBody.RecSourceType;
        }

        public Task ReloadTrainingDataAsync(ReloadBehavior behavior, bool finalize, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<ReloadTrainingDataRequest> operation = new Operation<ReloadTrainingDataRequest>(
                opName: OperationTypes.ReloadTrainingData,
                payload: new ReloadTrainingDataRequest(behavior, finalize)
            );
            return DoOperationWithoutResponseBodyAsync(operation, timeout, cancellationToken, descriptionForErrors: "reloading training data");
        }

        public Task FinalizeRecSourcesAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            Operation<FinalizeRecSourcesRequest> operation = new Operation<FinalizeRecSourcesRequest>(
                opName: OperationTypes.FinalizeRecSources,
                payload: new FinalizeRecSourcesRequest()
            );
            return DoOperationWithoutResponseBodyAsync(operation, timeout, cancellationToken, descriptionForErrors: "finalizing rec sources");
        }

        public Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsAsync(IDictionary<int, RecEngine.MAL.MalListEntry> animeList,
            string recSourceName, int numRecsDesired, decimal targetScore, TimeSpan timeout, CancellationToken cancellationToken)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OperationTypes.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetScore(recSourceName, numRecsDesired, targetScore, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendationsAsync(operation, timeout, cancellationToken);
        }

        public Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsWithPercentileTargetAsync(
            IDictionary<int, RecEngine.MAL.MalListEntry> animeList, string recSourceName, int numRecsDesired, decimal targetPercentile,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OperationTypes.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetFraction(recSourceName, numRecsDesired, targetPercentile, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendationsAsync(operation, timeout, cancellationToken);
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

        private async Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsAsync(Operation<GetMalRecsRequest> operation, TimeSpan timeout, CancellationToken cancellationToken)
        {
            GetMalRecsResponse response = await DoOperationWithResponseBodyAsync<GetMalRecsResponse>(
                operation, timeout, cancellationToken, descriptionForErrors: "getting recommendations").ConfigureAwait(false);

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="operation"></param>
        /// <param name="receiveTimeoutInMs"></param>
        /// <returns></returns>
        /// <exception cref="AnimeRecs.RecService.DTO.RecServiceErrorException">The recommendation service returned an error.
        /// Consult the ErrorCode property for more information.</exception>
        private async Task<TResponse> DoOperationAsync<TResponse>(Operation operation, TimeSpan receiveTimeout, CancellationToken cancellationToken, string descriptionForErrors)
            where TResponse : Response
        {
            try
            {
                string operationJsonString = JsonConvert.SerializeObject(operation);
                byte[] operationJsonBytes = Encoding.UTF8.GetBytes(operationJsonString);

                byte[] responseJsonBuffer;

                int length = operationJsonBytes.Length;
                int lengthNetworkOrder = IPAddress.HostToNetworkOrder(length);
                byte[] lengthBytes = BitConverter.GetBytes(lengthNetworkOrder);

                byte[] requestBytes = new byte[lengthBytes.Length + operationJsonBytes.Length];
                lengthBytes.CopyTo(requestBytes, index: 0);
                operationJsonBytes.CopyTo(requestBytes, index: lengthBytes.Length);

                const int sendTimeoutInSeconds = 3;
                TimeSpan sendTimeout = TimeSpan.FromSeconds(sendTimeoutInSeconds);

                using (Socket socket = await CreateSocketAsync(cancellationToken).ConfigureAwait(false))
                {
                    Logging.Log.Trace("Sending bytes.");
                    await socket.SendAllAsync(requestBytes, sendTimeout, cancellationToken).ConfigureAwait(false);
                    Logging.Log.Trace("Sent bytes.");

                    byte[] responseLengthBuffer = await socket.ReceiveAllAsync(4, receiveTimeout, cancellationToken).ConfigureAwait(false);
                    int responseLengthNetworkOrder = BitConverter.ToInt32(responseLengthBuffer, 0);
                    int responseLength = IPAddress.NetworkToHostOrder(responseLengthNetworkOrder);

                    // Now that we got the length of the response, the service is done processing the request and has sent the response,
                    // so the same timeout for receiving the rest of the response can be used for all operations.
                    const int receiveTimeoutForRestOfResponseInSeconds = 5;
                    TimeSpan receiveTimeoutForRestOfResponse = TimeSpan.FromSeconds(receiveTimeoutForRestOfResponseInSeconds);
                    responseJsonBuffer = await socket.ReceiveAllAsync(responseLength, receiveTimeoutForRestOfResponse, cancellationToken).ConfigureAwait(false);
                    Logging.Log.Trace("Got response.");
                }

                string responseJsonString = Encoding.UTF8.GetString(responseJsonBuffer);
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
            catch (RecServiceErrorException ex)
            {
                string message = string.Format("Error {0}: {1}", descriptionForErrors, ex.Error.Message);
                throw new RecServiceErrorException(new Error(ex.Error.ErrorCode, message), ex);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error {0}: {1}", descriptionForErrors, ex.Message), ex);
            }
        }

        private async Task<TResponseBody> DoOperationWithResponseBodyAsync<TResponseBody>(Operation operation, TimeSpan timeout, CancellationToken cancellationToken, string descriptionForErrors)
        {
            Response<TResponseBody> response = await DoOperationAsync<Response<TResponseBody>>(operation, timeout, cancellationToken, descriptionForErrors).ConfigureAwait(false);
            return response.Body;
        }

        private Task DoOperationWithoutResponseBodyAsync(Operation operation, TimeSpan timeout, CancellationToken cancellationToken, string descriptionForErrors)
        {
            return DoOperationAsync<Response>(operation, timeout, cancellationToken, descriptionForErrors);
        }

        private async Task<Socket> CreateSocketAsync(CancellationToken cancellationToken)
        {
            Logging.Log.Trace("Connecting to rec service.");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Hardcoded constant connect timeout, reasonable whether local or remote.
            const int connectTimeoutInSeconds = 3;
            TimeSpan connectTimeout = TimeSpan.FromSeconds(connectTimeoutInSeconds);

            Task connectTask = socket.ConnectAsync(AnimeRecsServiceAddress, PortNumber, connectTimeout, cancellationToken);

            try
            {
                await connectTask.ConfigureAwait(false);
            }
            catch (SocketTimeoutException ex)
            {
                throw new SocketTimeoutException(string.Format("Timeout connecting to rec service at {0}:{1}.", AnimeRecsServiceAddress, PortNumber), ex);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                throw new Exception(string.Format("Error connecting to rec service at {0}:{1}: {2}", AnimeRecsServiceAddress, PortNumber, ex.Message), ex);
            }

            Logging.Log.Trace("Connected to rec service.");
            return socket;
        }

        public void Dispose()
        {
            ;
        }
    }
}

// Copyright (C) 2017 Greg Najda
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