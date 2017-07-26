using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.OperationHandlers;
using AnimeRecs.Utils;
using System.Threading;
using System.Threading.Tasks;

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
                operationHandler: OpHandlers.PingAsync,
                operationType: typeof(Operation<PingRequest>),
                responseType: typeof(Response<PingResponse>)
                )
            },

            { OpNames.LoadRecSource, new OperationDescription
                (
                operationHandler: OpHandlers.LoadRecSourceAsync,
                operationType: typeof(Operation<LoadRecSourceRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.UnloadRecSource, new OperationDescription
                (
                operationHandler: OpHandlers.UnloadRecSourceAsync,
                operationType: typeof(Operation<UnloadRecSourceRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.GetRecSourceType, new OperationDescription
                (
                operationHandler: OpHandlers.GetRecSourceTypeAsync,
                operationType: typeof(Operation<GetRecSourceTypeRequest>),
                responseType: typeof(Response<GetRecSourceTypeResponse>)
                )
            },

            { OpNames.ReloadTrainingData, new OperationDescription
                (
                operationHandler: OpHandlers.ReloadTrainingDataAsync,
                operationType: typeof(Operation<ReloadTrainingDataRequest>),
                responseType: typeof(Response)
                )
            },

            { OpNames.GetMalRecs, new OperationDescription
                (
                operationHandler: OpHandlers.GetMalRecsAsync,
                operationType: typeof(Operation<GetMalRecsRequest>),
                responseType: typeof(Response<GetMalRecsResponse>)
                )
            },

            { OpNames.FinalizeRecSources, new OperationDescription
                (
                operationHandler: OpHandlers.FinalizeRecSourcesAsync,
                operationType: typeof(Operation<FinalizeRecSourcesRequest>),
                responseType: typeof(Response)
                )
            }
        };

        private Socket m_socket;
        private RecServiceState m_state;
        private TimeSpan m_readTimeout;
        private TimeSpan m_writeTimeout;
        private CancellationToken m_cancellationToken;

        public ConnectionServicer(Socket clientSocket, RecServiceState state, TimeSpan readTimeout, TimeSpan writeTimeout, CancellationToken cancellationToken)
        {
            m_socket = clientSocket;
            m_state = state;
            m_readTimeout = readTimeout;
            m_writeTimeout = writeTimeout;
            m_cancellationToken = cancellationToken;
        }

        public async Task ServiceConnectionAsync()
        {
            try
            {
                await ServiceConnectionCoreAsync().ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException) && !(ex is SocketTimeoutException) && !(ex is SocketException))
            {
                try
                {
                    await SendUnexpectedErrorAsync(ex).ConfigureAwait(false);
                }
                catch (Exception ex2) when (!(ex is OperationCanceledException))
                {
                    Logging.Log.InfoFormat("Error trying to notify client of unexpected error: {0}", ex2, ex2.Message);
                }

                throw;
            }
        }

        private async Task ServiceConnectionCoreAsync()
        {
            Logging.Log.Debug("Reading message from client.");

            // Read 4 bytes for the message length. Convert from network to host byte order.
            byte[] messageLengthBytes = new byte[4];
            await m_socket.ReceiveAllAsync(messageLengthBytes, m_readTimeout, m_cancellationToken).ConfigureAwait(false);
            int messageLengthNetworkOrder = BitConverter.ToInt32(messageLengthBytes, 0);
            int messageLength = IPAddress.NetworkToHostOrder(messageLengthNetworkOrder);
            Logging.Log.TraceFormat("Message length is {0} bytes, reading the rest of the message now.", messageLength);

            // Read the rest of the message.
            byte[] messageBytes = new byte[messageLength];
            await m_socket.ReceiveAllAsync(messageBytes,m_readTimeout, m_cancellationToken).ConfigureAwait(false);

            Logging.Log.Trace("Converting message bytes into string.");
            string messageString = Encoding.UTF8.GetString(messageBytes);

            Operation operation;
            try
            {
                Logging.Log.Trace("Deserializing message.");
                operation = JsonConvert.DeserializeObject<Operation>(messageString);
            }
            catch (JsonReaderException ex)
            {
                await SendInvalidJsonErrorAsync(ex).ConfigureAwait(false);
                return;
            }

            Logging.Log.DebugFormat("Got message with OpName {0}", operation.OpName);

            if (operation.OpName == null)
            {
                await SendNoOpErrorAsync().ConfigureAwait(false);
                return;
            }

            if (!Operations.ContainsKey(operation.OpName))
            {
                await SendBadOpErrorAsync(operation.OpName).ConfigureAwait(false);
                return;
            }

            OperationDescription opDescription = Operations[operation.OpName];

            try
            {
                Response response = await opDescription.OperationHandler(operation, m_state, m_cancellationToken).ConfigureAwait(false);
                Logging.Log.Debug("Operation completed, writing response.");
                await WriteResponseAsync(response);
            }
            catch (RecServiceErrorException ex)
            {
                Response errorResponse = new Response(ex.Error);
                await WriteResponseAsync(errorResponse);
            }
        }

        private Task SendInvalidJsonErrorAsync(JsonReaderException ex)
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.InvalidMessage,
                    Message = string.Format("Invalid message. Expected a JSON object. {0}", ex.Message)
                }
            };

            return WriteResponseAsync(errorResponse);
        }

        private Task SendNoOpErrorAsync()
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.InvalidMessage,
                    Message = "No OpName specified."
                }
            };
            return WriteResponseAsync(errorResponse);
        }

        private Task SendBadOpErrorAsync(string opName)
        {
            Response errorResponse = new Response()
            {
                Error = new Error()
                {
                    ErrorCode = ErrorCodes.NoSuchOp,
                    Message = string.Format("'{0}' is not a valid OpName.", opName)
                }
            };
            return WriteResponseAsync(errorResponse);
        }

        private Task SendUnexpectedErrorAsync(Exception ex)
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
            return WriteResponseAsync(errorResponse);
        }

        private async Task WriteResponseAsync(Response response)
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

            Logging.Log.Trace("Writing response length.");
            await m_socket.SendAllAsync(responseLengthBytes, m_writeTimeout, m_cancellationToken).ConfigureAwait(false);
            Logging.Log.Trace("Writing the rest of the response.");
            await m_socket.SendAllAsync(responseJsonBytes, m_writeTimeout, m_cancellationToken).ConfigureAwait(false);
            Logging.Log.Debug("Response written.");
        }
    }
}

// Copyright (C) 2017 Greg Najda
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