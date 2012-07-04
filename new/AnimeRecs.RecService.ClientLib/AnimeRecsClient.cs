using System;
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
    /// <summary>
    /// Client to the rec service. This class is thread-safe.
    /// </summary>
    public class AnimeRecsClient : IDisposable
    {
        private int PortNumber { get; set; }

        public static readonly int DefaultPort = 5541;

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

        public void ReloadTrainingData(int receiveTimeoutInMs = 0)
        {
            Operation operation = new Operation(OpNames.ReloadTrainingData);
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
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
            string jsonResponseString;
            GetMalRecsResponse<Recommendation> response = DoOperationWithResponseBody<GetMalRecsResponse<Recommendation>>(operation, receiveTimeoutInMs, out jsonResponseString);

            IEnumerable<IRecommendation> results;

            if (response.RecommendationType.Equals(RecommendationTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.AverageScoreRecommendation>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.AverageScoreRecommendation>>>(jsonResponseString);

                List<IRecommendation> recommendations = new List<IRecommendation>();
                foreach (DTO.AverageScoreRecommendation dtoRec in specificResponse.Body.Recommendations)
                {
                    recommendations.Add(new AnimeRecs.RecEngine.AverageScoreRecommendation(dtoRec.MalAnimeId, dtoRec.NumRatings, dtoRec.AverageScore));
                }

                results = recommendations;
            }
            else if (response.RecommendationType.Equals(RecommendationTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.MostPopularRecommendation>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.MostPopularRecommendation>>>(jsonResponseString);

                List<IRecommendation> recommendations = new List<IRecommendation>();
                foreach (DTO.MostPopularRecommendation dtoRec in specificResponse.Body.Recommendations)
                {
                    recommendations.Add(new AnimeRecs.RecEngine.MostPopularRecommendation(
                        itemId: dtoRec.MalAnimeId,
                        popularityRank: dtoRec.PopularityRank,
                        numRatings: dtoRec.NumRatings
                    ));
                }

                results = recommendations;
            }
            else if (response.RecommendationType.Equals(RecommendationTypes.RatingPrediction, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.RatingPredictionRecommendation>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.RatingPredictionRecommendation>>>(jsonResponseString);

                List<IRecommendation> recommendations = new List<IRecommendation>();
                foreach (DTO.RatingPredictionRecommendation dtoRec in specificResponse.Body.Recommendations)
                {
                    recommendations.Add(new RecEngine.RatingPredictionRecommendation(
                        itemId: dtoRec.MalAnimeId,
                        predictedRating: dtoRec.PredictedRating
                    ));
                }

                results = recommendations;
            }
            else if (response.RecommendationType.Equals(RecommendationTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
            {
                Response<GetMalRecsResponse<DTO.AnimeRecsRecommendation, DTO.MalAnimeRecsExtraResponseData>> specificResponse =
                    JsonConvert.DeserializeObject<Response<GetMalRecsResponse<DTO.AnimeRecsRecommendation, DTO.MalAnimeRecsExtraResponseData>>>(jsonResponseString);

                List<RecEngine.AnimeRecsRecommendation> recommendations = new List<RecEngine.AnimeRecsRecommendation>();
                foreach (DTO.AnimeRecsRecommendation dtoRec in specificResponse.Body.Recommendations)
                {
                    recommendations.Add(new RecEngine.AnimeRecsRecommendation(dtoRec.RecommenderUserId, itemId: dtoRec.MalAnimeId));
                }

                List<MalAnimeRecsRecommenderUser> recommenders = new List<MalAnimeRecsRecommenderUser>();
                foreach (DTO.MalAnimeRecsRecommender dtoRecommender in specificResponse.Body.Data.Recommenders)
                {
                    HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                            dtoRecommender.Recs.Where(rec => rec.Liked.HasValue && rec.Liked.Value == true)
                            .Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore)));

                    HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsNotLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                            dtoRecommender.Recs.Where(rec => rec.Liked.HasValue && rec.Liked.Value == false)
                            .Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore)));

                    recommenders.Add(new MalAnimeRecsRecommenderUser(
                        userId: dtoRecommender.UserId,
                        username: dtoRecommender.Username,
                        recsLiked: recsLiked,
                        recsNotLiked: recsNotLiked,
                        allRecommendations: new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                            dtoRecommender.Recs.Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore))),
                        compatibility: dtoRecommender.Compatibility,
                        compatibilityLowEndpoint: dtoRecommender.CompatibilityLowEndpoint,
                        compatibilityHighEndpoint: dtoRecommender.CompatibilityHighEndpoint
                    ));
                }

                results = new RecEngine.MAL.MalAnimeRecsResults(recommendations, recommenders, specificResponse.Body.Data.TargetScoreUsed);
            }
            else
            {
                List<IRecommendation> recommendations = new List<IRecommendation>();
                foreach (DTO.Recommendation dtoRec in response.Recommendations)
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

            MalRecResults<IEnumerable<IRecommendation>> resultsWithAnimes = new MalRecResults<IEnumerable<IRecommendation>>(results, animes);

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