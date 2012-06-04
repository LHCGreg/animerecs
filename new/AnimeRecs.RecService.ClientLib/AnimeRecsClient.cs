using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using AnimeRecs.RecService.DTO;
using Newtonsoft.Json;
using MiscUtil.IO;

namespace AnimeRecs.RecService.ClientLib
{
    public class AnimeRecsClient : IDisposable
    {
        private int PortNumber { get; set; }

        public AnimeRecsClient(int portNumber)
        {
            PortNumber = portNumber;
        }

        /// <exception cref="AnimeRecs.RecService.ClientLib.RecServiceException">The recommendation service returned an error.
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

        public void ReloadTrainingData(int receiveTimeoutInMs = 0)
        {
            Operation operation = new Operation(OpNames.ReloadTrainingData);
            DoOperationWithoutResponseBody(operation, receiveTimeoutInMs: receiveTimeoutInMs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="operation"></param>
        /// <param name="receiveTimeoutInMs"></param>
        /// <returns></returns>
        /// <exception cref="AnimeRecs.RecService.ClientLib.RecServiceException">The recommendation service returned an error.
        /// Consult the ErrorCode property for more information.</exception>
        private TResponse DoOperation<TResponse>(Operation operation, int receiveTimeoutInMs)
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

            string responseJsonString = Encoding.UTF8.GetString(responseJsonBytes);
            TResponse response = JsonConvert.DeserializeObject<TResponse>(responseJsonString);
            if (response.Error != null)
            {
                throw new RecServiceException(response.Error);
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