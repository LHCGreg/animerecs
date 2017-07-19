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
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Nito.AsyncEx;
using System.IO;

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
        public async Task<string> PingAsync(string message, CancellationToken cancellationToken)
        {
            Operation<PingRequest> operation = new Operation<PingRequest>(
                opName: OpNames.Ping,
                payload: new PingRequest(message)
            );

            PingResponse pingResponse = await DoOperationWithResponseBodyAsync<PingResponse>(operation, cancellationToken).ConfigureAwait(false);
            return pingResponse.ResponseMessage;
        }

        public Task LoadRecSourceAsync(string name, bool replaceExisting, RecSourceParams parameters, CancellationToken cancellationToken)
        {
            Operation<LoadRecSourceRequest<RecSourceParams>> operation = new Operation<LoadRecSourceRequest<RecSourceParams>>(
                opName: OpNames.LoadRecSource,
                payload: new LoadRecSourceRequest<RecSourceParams>(
                    name: name, type: parameters.GetRecSourceTypeName(), replaceExisting: replaceExisting, parameters: parameters
                )
            );

            return DoOperationWithoutResponseBodyAsync(operation, cancellationToken);
        }

        public Task LoadRecSourceAsync(LoadRecSourceRequest request, CancellationToken cancellationToken)
        {
            Operation<LoadRecSourceRequest> operation = new Operation<LoadRecSourceRequest>(
                opName: OpNames.LoadRecSource,
                payload: request
            );

            return DoOperationWithoutResponseBodyAsync(operation, cancellationToken);
        }

        public Task UnloadRecSourceAsync(string name, CancellationToken cancellationToken)
        {
            Operation<UnloadRecSourceRequest> operation = new Operation<UnloadRecSourceRequest>(
                opName: OpNames.UnloadRecSource,
                payload: new UnloadRecSourceRequest(name)
            );

            return DoOperationWithoutResponseBodyAsync(operation, cancellationToken);
        }

        public async Task<string> GetRecSourceTypeAsync(string recSourceName, CancellationToken cancellationToken)
        {
            Operation<GetRecSourceTypeRequest> operation = new Operation<GetRecSourceTypeRequest>(
                opName: OpNames.GetRecSourceType,
                payload: new GetRecSourceTypeRequest(recSourceName)
            );

            GetRecSourceTypeResponse responseBody = await DoOperationWithResponseBodyAsync<GetRecSourceTypeResponse>(operation, cancellationToken).ConfigureAwait(false);
            return responseBody.RecSourceType;
        }

        public Task ReloadTrainingDataAsync(ReloadBehavior behavior, bool finalize, CancellationToken cancellationToken)
        {
            Operation<ReloadTrainingDataRequest> operation = new Operation<ReloadTrainingDataRequest>(
                opName: OpNames.ReloadTrainingData,
                payload: new ReloadTrainingDataRequest(behavior, finalize)
            );
            return DoOperationWithoutResponseBodyAsync(operation, cancellationToken);
        }

        public Task FinalizeRecSourcesAsync(CancellationToken cancellationToken)
        {
            Operation<FinalizeRecSourcesRequest> operation = new Operation<FinalizeRecSourcesRequest>(
                opName: OpNames.FinalizeRecSources,
                payload: new FinalizeRecSourcesRequest()
            );
            return DoOperationWithoutResponseBodyAsync(operation, cancellationToken);
        }

        public Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsAsync(IDictionary<int, RecEngine.MAL.MalListEntry> animeList,
            string recSourceName, int numRecsDesired, decimal targetScore, CancellationToken cancellationToken)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OpNames.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetScore(recSourceName, numRecsDesired, targetScore, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendationsAsync(operation, cancellationToken);
        }

        public Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsWithPercentileTargetAsync(
            IDictionary<int, RecEngine.MAL.MalListEntry> animeList, string recSourceName, int numRecsDesired, decimal targetPercentile,
            CancellationToken cancellationToken)
        {
            List<DTO.MalListEntry> dtoAnimeList = CreateDtoAnimeList(animeList);

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OpNames.GetMalRecs,
                GetMalRecsRequest.CreateWithTargetFraction(recSourceName, numRecsDesired, targetPercentile, new MalListForUser(dtoAnimeList)));

            return GetMalRecommendationsAsync(operation, cancellationToken);
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

        private async Task<MalRecResults<IEnumerable<IRecommendation>>> GetMalRecommendationsAsync(Operation<GetMalRecsRequest> operation, CancellationToken cancellationToken)
        {
            GetMalRecsResponse response = await DoOperationWithResponseBodyAsync<GetMalRecsResponse>(operation, cancellationToken).ConfigureAwait(false);

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
        private async Task<TResponse> DoOperationAsync<TResponse>(Operation operation, CancellationToken cancellationToken)
            where TResponse : Response
        {
            string operationJsonString = JsonConvert.SerializeObject(operation);
            byte[] operationJsonBytes = Encoding.UTF8.GetBytes(operationJsonString);

            byte[] responseJsonBuffer;

            int length = operationJsonBytes.Length;
            int lengthNetworkOrder = IPAddress.HostToNetworkOrder(length);
            byte[] lengthBytes = BitConverter.GetBytes(lengthNetworkOrder);

            using (Socket socket = await CreateSocketAsync(cancellationToken).ConfigureAwait(false))
            {
                Logging.Log.Trace("Sending bytes.");
                await SocketSendAllAsync(socket, lengthBytes, cancellationToken);
                await SocketSendAllAsync(socket, operationJsonBytes, cancellationToken);
                Logging.Log.Trace("Sent bytes.");

                byte[] responseLengthBuffer = await SocketReceiveAllAsync(socket, 4, cancellationToken).ConfigureAwait(false);
                int responseLengthNetworkOrder = BitConverter.ToInt32(responseLengthBuffer, 0);
                int responseLength = IPAddress.NetworkToHostOrder(responseLengthNetworkOrder);

                responseJsonBuffer = await SocketReceiveAllAsync(socket, responseLength, cancellationToken).ConfigureAwait(false);
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

        private async Task<TResponseBody> DoOperationWithResponseBodyAsync<TResponseBody>(Operation operation, CancellationToken cancellationToken)
        {
            Response<TResponseBody> response = await DoOperationAsync<Response<TResponseBody>>(operation, cancellationToken).ConfigureAwait(false);
            return response.Body;
        }

        private Task DoOperationWithoutResponseBodyAsync(Operation operation, CancellationToken cancellationToken)
        {
            return DoOperationAsync<Response>(operation, cancellationToken);
        }

        private async Task<Socket> CreateSocketAsync(CancellationToken cancellationToken)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Logging.Log.Trace("Connecting to rec service.");
            Task connectTask = socket.ConnectAsync(IPAddress.Loopback, PortNumber);
            try
            {
                await connectTask.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                try
                {
                    socket.Dispose();
                }
                catch
                {

                }
                throw;
            }
            Logging.Log.Trace("Connected to rec service.");
            return socket;
        }

        private async Task SocketSendAllAsync(Socket socket, byte[] bytes, CancellationToken cancellationToken)
        {
            int numBytesSent = 0;
            while (numBytesSent < bytes.Length)
            {
                Task<int> sendTask = socket.SendAsync(new ArraySegment<byte>(bytes, offset: numBytesSent, count: bytes.Length - numBytesSent), SocketFlags.None);

                // Add cancellation functionality to SendAsync.
                // If it does get canceled, an OperationCanceledException will be thrown.
                // The code a bit higher up will dispose the socket, which is the only sensible thing to do
                // if you cancel in the middle of a send.
                int numSentThisTime = await sendTask.WaitAsync(cancellationToken).ConfigureAwait(false);

                numBytesSent += numSentThisTime;
            }
        }

        private async Task<byte[]> SocketReceiveAllAsync(Socket socket, int numBytesToReceive, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[numBytesToReceive];
            await SocketReceiveAllAsync(socket, buffer, offset: 0, numBytesToReceive: numBytesToReceive, cancellationToken: cancellationToken).ConfigureAwait(false);
            return buffer;
        }

        private async Task SocketReceiveAllAsync(Socket socket, byte[] buffer, int offset, int numBytesToReceive, CancellationToken cancellationToken)
        {
            int numBytesReceived = 0;
            while (numBytesReceived < numBytesToReceive)
            {
                Task<int> receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset: offset + numBytesReceived, count: numBytesToReceive - numBytesReceived), SocketFlags.None);
                // Add cancellation functionality to ReceiveAsync.
                // If it does get canceled, an OperationCanceledException will be thrown.
                // The code a bit higher up will dispose the socket, which is the only sensible thing to do
                // if you cancel in the middle of a receive.

                int numReceivedThisTime = await receiveTask.WaitAsync(cancellationToken).ConfigureAwait(false);
                if (numReceivedThisTime == 0)
                {
                    throw new EndOfStreamException(string.Format("Expected the remote end to send {0} bytes but only received {1} bytes.", numBytesToReceive, numBytesReceived));
                }

                numBytesReceived += numReceivedThisTime;
            }
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