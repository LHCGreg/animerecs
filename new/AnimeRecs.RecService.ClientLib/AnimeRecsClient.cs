﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using AnimeRecs.RecService.DTO;
using Newtonsoft.Json;
using MiscUtil.IO;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.ClientLib
{
    public class AnimeRecsClient : IDisposable
    {
        private int PortNumber { get; set; }

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

        public void LoadAverageScoreRecSource(string name, bool replaceExisting, AverageScoreRecSourceParams parameters, int receiveTimeoutInMs = 0)
        {
            Operation<LoadRecSourceRequest<AverageScoreRecSourceParams>> operation = new Operation<LoadRecSourceRequest<AverageScoreRecSourceParams>>(
                opName: OpNames.LoadRecSource,
                payload: new LoadRecSourceRequest<AverageScoreRecSourceParams>(
                    name: name, type: RecSourceTypes.AverageScore, replaceExisting: replaceExisting, parameters: parameters
                )
            );

            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public void LoadMostPopularRecSource(string name, bool replaceExisting, MostPopularRecSourceParams parameters, int receiveTimeoutInMs = 0)
        {
            Operation<LoadRecSourceRequest<MostPopularRecSourceParams>> operation = new Operation<LoadRecSourceRequest<MostPopularRecSourceParams>>(
                opName: OpNames.LoadRecSource,
                payload: new LoadRecSourceRequest<MostPopularRecSourceParams>(
                    name: name, type: RecSourceTypes.MostPopular, replaceExisting: replaceExisting, parameters: parameters
                )
            );

            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public void LoadAnimeRecsRecSource(string name, bool replaceExisting, AnimeRecsRecSourceParams parameters, int receiveTimeoutInMs = 0)
        {
            Operation<LoadRecSourceRequest<AnimeRecsRecSourceParams>> operation = new Operation<LoadRecSourceRequest<AnimeRecsRecSourceParams>>(
                opName: OpNames.LoadRecSource,
                payload: new LoadRecSourceRequest<AnimeRecsRecSourceParams>(
                    name: name, type: RecSourceTypes.AnimeRecs, replaceExisting: replaceExisting, parameters: parameters
                )
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

        public void ReloadTrainingData(int receiveTimeoutInMs = 0)
        {
            Operation operation = new Operation(OpNames.ReloadTrainingData);
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        public MalRecommendations GetMalRecommendations(IDictionary<int, RecEngine.MAL.MalListEntry> animeList, string recSourceName,
            int numRecsDesired, decimal targetScore, int receiveTimeoutInMs = 0)
        {
            List<DTO.MalListEntry> dtoAnimeList = new List<DTO.MalListEntry>();
            foreach (int animeId in animeList.Keys)
            {
                RecEngine.MAL.MalListEntry engineEntry = animeList[animeId];
                DTO.MalListEntry dtoEntry = new DTO.MalListEntry(animeId, engineEntry.Rating, engineEntry.Status, engineEntry.NumEpisodesWatched);
                dtoAnimeList.Add(dtoEntry);
            }

            Operation<GetMalRecsRequest> operation = new Operation<GetMalRecsRequest>(OpNames.GetMalRecs,
                new GetMalRecsRequest(recSourceName, numRecsDesired, targetScore, new MalListForUser(dtoAnimeList)));
            string jsonResponseString;
            GetMalRecsResponse<Recommendation> response = DoOperationWithResponseBody<GetMalRecsResponse<Recommendation>>(operation, receiveTimeoutInMs, out jsonResponseString);

            List<IRecommendation> recommendations = new List<IRecommendation>();
            if (response.RecommendationType.Equals(RecommendationTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.AverageScoreRecommendation>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.AverageScoreRecommendation>>>(jsonResponseString);
                recommendations.AddRange(specificResponse.Body.Recommendations.Select(
                    dtoRec => new AnimeRecs.RecEngine.AverageScoreRecommendation(dtoRec.MalAnimeId, dtoRec.NumRatings, dtoRec.AverageScore)));
            }
            else if (response.RecommendationType.Equals(RecommendationTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.MostPopularRecommendation>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.MostPopularRecommendation>>>(jsonResponseString);
                recommendations.AddRange(specificResponse.Body.Recommendations.Select(
                    dtoRec => new AnimeRecs.RecEngine.MostPopularRecommendation(dtoRec.MalAnimeId, dtoRec.PopularityRank, dtoRec.NumRatings)));
            }
            else
            {
                recommendations.AddRange(response.Recommendations.Select(dtoRec => new BasicRecommendation(dtoRec.MalAnimeId)));
            }

            Dictionary<int, MalAnime> animeInfo = new Dictionary<int, MalAnime>();
            foreach (Recommendation basicRecDto in response.Recommendations)
            {
                animeInfo[basicRecDto.MalAnimeId] = new MalAnime(basicRecDto.MalAnimeId, basicRecDto.MalAnimeType, basicRecDto.Title);
            }

            return new MalRecommendations(recommendations, animeInfo);
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
                socket.Client.Send(operationJsonBytes);
                using (NetworkStream socketStream = socket.GetStream())
                {
                    // The half-close must be after getting the NetworkStream. GetStream() will throw an exception if the connection is half-closed.
                    // The half-close must also be before reading the response. The service will not start processing the message
                    // until the connection is half-closed.
                    socket.Client.Shutdown(SocketShutdown.Send);
                    responseJsonBytes = StreamUtil.ReadFully(socketStream);
                }
            }

            responseJsonString = Encoding.UTF8.GetString(responseJsonBytes);
            TResponse response = JsonConvert.DeserializeObject<TResponse>(responseJsonString);
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

        private TResponseBody DoOperationWithResponseBody<TResponseBody>(Operation operation, int receiveTimeoutInMs, out string jsonResponseString)
        {
            Response<TResponseBody> response = DoOperation<Response<TResponseBody>>(operation, receiveTimeoutInMs, out jsonResponseString);
            return response.Body;
        }

        private void DoOperationWithoutResponseBody(Operation operation, int receiveTimeoutInMs)
        {
            Response response = DoOperation<Response>(operation, receiveTimeoutInMs);
        }

        private TcpClient CreateTcpClient(int receiveTimeoutInMs)
        {
            TcpClient client = new TcpClient("localhost", PortNumber);
            client.SendTimeout = 3000;
            client.ReceiveTimeout = receiveTimeoutInMs;
            return client;
        }

        public void Dispose()
        {
            ;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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