using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MiscUtil.IO;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.OperationHandlers;

namespace AnimeRecs.RecService
{
    /// <summary>
    /// Services one connection's request.
    /// </summary>
    internal class ConnectionServicer
    {
        private static Dictionary<string, OperationDescription> Operations = new Dictionary<string, OperationDescription>(StringComparer.OrdinalIgnoreCase)
        {
            { OpNames.Ping, new OperationDescription
                (
                operationHandler: OpHandlers.Ping,
                operationType: typeof(Operation<PingRequest>),
                responseType: typeof(Response<PingResponse>)
                )
            },

            { OpNames.LoadRecSource, new OperationDescription
                (
                operationHandler: OpHandlers.LoadRecSource,
                operationType: typeof(Operation<LoadRecSourceRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.UnloadRecSource, new OperationDescription
                (
                operationHandler: OpHandlers.UnloadRecSource,
                operationType: typeof(Operation<UnloadRecSourceRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.GetRecSourceType, new OperationDescription
                (
                operationHandler: OpHandlers.GetRecSourceType,
                operationType: typeof(Operation<GetRecSourceTypeRequest>),
                responseType: typeof(Response<GetRecSourceTypeResponse>)
                )
            },

            { OpNames.ReloadTrainingData, new OperationDescription
                (
                operationHandler: OpHandlers.ReloadTrainingData,
                operationType: typeof(Operation<ReloadTrainingDataRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.GetMalRecs, new OperationDescription
                (
                operationHandler: OpHandlers.GetMalRecs,
                operationType: typeof(Operation<GetMalRecsRequest>),
                responseType: typeof(Response<GetMalRecsResponse>)
                )
            },

            { OpNames.FinalizeRecSources, new OperationDescription
                (
                operationHandler: OpHandlers.FinalizeRecSources,
                operationType: typeof(Operation<FinalizeRecSourcesRequest>),
                responseType: typeof(Response)
                )
            }
        };
        
        private TcpClient Client { get; set; }
        private RecServiceState State { get; set; }
        
        public ConnectionServicer(TcpClient client, RecServiceState state)
        {
            Client = client;
            State = state;
        }

        public void ServiceConnection()
        {
            try
            {
                ServiceConnectionCore();
            }
            catch (Exception ex)
            {
                try
                {
                    SendUnexpectedError(ex);
                }
                catch (Exception ex2)
                {
                    Logging.Log.InfoFormat("Error trying to notify client of unexpected error: {0}", ex2, ex2.Message);
                }

                throw;
            }
        }

        private void ServiceConnectionCore()
        {
            Logging.Log.Debug("Reading message from client.");
            byte[] messageBytes;
            using (NetworkStream clientStream = new NetworkStream(Client.Client, ownsSocket: false))
            {
                byte[] messageLengthBytes = StreamUtil.ReadExactly(clientStream, 4);
                int messageLengthNetworkOrder = BitConverter.ToInt32(messageLengthBytes, 0);
                int messageLength = IPAddress.NetworkToHostOrder(messageLengthNetworkOrder);
                messageBytes = StreamUtil.ReadExactly(clientStream, messageLength);
            }

            Logging.Log.Debug("Converting message bytes into string.");
            string messageString = Encoding.UTF8.GetString(messageBytes);

            Operation operation;
            try
            {
                Logging.Log.Debug("Deserializing message.");
                operation = JsonConvert.DeserializeObject<Operation>(messageString);
            }
            catch (JsonReaderException ex)
            {
                SendInvalidJsonError(ex);
                return;
            }

            Logging.Log.DebugFormat("Got message with OpName {0}", operation.OpName);

            if (operation.OpName == null)
            {
                SendNoOpError();
                return;
            }

            if(!Operations.ContainsKey(operation.OpName))
            {
                SendBadOpError(operation.OpName);
                return;
            }

            OperationDescription opDescription = Operations[operation.OpName];

            try
            {
                Response response = opDescription.OperationHandler(operation, State);
                Logging.Log.Debug("Operation completed, writing response.");
                WriteResponse(response);
            }
            catch (RecServiceErrorException ex)
            {
                Response errorResponse = new Response(ex.Error);
                WriteResponse(errorResponse);
            }
        }

        private void SendInvalidJsonError(JsonReaderException ex)
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.InvalidMessage,
                    Message = string.Format("Invalid message. Expected a JSON object. {0}", ex.Message)
                }
            };
            WriteResponse(errorResponse);
        }

        private void SendNoOpError()
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.InvalidMessage,
                    Message = "No OpName specified."
                }
            };
            WriteResponse(errorResponse);
        }

        private void SendBadOpError(string opName)
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.NoSuchOp,
                    Message = string.Format("'{0}' is not a valid OpName.", opName)
                }
            };
            WriteResponse(errorResponse);
        }

        private void SendUnexpectedError(Exception ex)
        {
            Logging.Log.ErrorFormat("Unexpected error while servicing connection: {0}", ex, ex.Message);
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.Unknown,
                    Message = string.Format("Unexpected error: {0}.", ex.Message)
                }
            };
            WriteResponse(errorResponse);
        }

        private void WriteResponse(Response response)
        {
            if (response.Error != null)
            {
                Logging.Log.InfoFormat("Sending error response with message: {0}", response.Error.Message);
            }
            
            Logging.Log.Trace("Serializing response.");
            string responseJsonString = JsonConvert.SerializeObject(response);

            Logging.Log.Trace("Turning response string into bytes.");
            byte[] responseJsonBytes = Encoding.UTF8.GetBytes(responseJsonString);
            int responseLength = responseJsonBytes.Length;
            int responseLengthNetworkOrder = IPAddress.HostToNetworkOrder(responseLength);
            byte[] responseLengthBytes = BitConverter.GetBytes(responseLengthNetworkOrder);

            Logging.Log.Trace("Writing response.");
            SocketSendAll(responseLengthBytes);
            SocketSendAll(responseJsonBytes);
            Logging.Log.Debug("Response written.");
        }

        private void SocketSendAll(byte[] bytes)
        {
            int numSent = 0;
            while (numSent < bytes.Length)
            {
                // socketFlags parameter is currently named incorrectly in mono - see https://bugzilla.xamarin.com/show_bug.cgi?id=25169
                int numSentThisTime = Client.Client.Send(bytes, /*offset:*/ numSent, /*size:*/ bytes.Length - numSent, /*socketFlags:*/ SocketFlags.None);
                numSent += numSentThisTime;
            }
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.