using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AnimeRecs.RecService.DTO;
using MiscUtil.IO;
using Newtonsoft.Json;
using AnimeRecs.RecService.OperationHandlers;

namespace AnimeRecs.RecService
{
    /// <summary>
    /// Services one connection's request.
    /// </summary>
    internal class ConnectionServicer
    {
        private Dictionary<string, OperationDescription> Operations = new Dictionary<string, OperationDescription>(StringComparer.OrdinalIgnoreCase)
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
                operationType: typeof(Operation<LoadRecSourceRequest<RecSourceParams>>),
                responseType: typeof(Response)
                )
            },

            { OpNames.ReloadTrainingData, new OperationDescription
                (
                operationHandler: OpHandlers.ReloadTrainingData,
                operationType: typeof(Operation),
                responseType: typeof(Response)
                )
            },

            { OpNames.GetMalRecs, new OperationDescription
                (
                operationHandler: OpHandlers.GetMalRecs,
                operationType: typeof(Operation<GetMalRecsRequest>),
                responseType: typeof(Response<GetMalRecsResponse>)
                )
            }
        };
        
        private Stream ClientStream { get; set; }
        private RecServiceState State { get; set; }
        
        public ConnectionServicer(Stream clientStream, RecServiceState state)
        {
            ClientStream = clientStream;
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
                    // TODO: Logging
                    Console.WriteLine("Error trying to notify client of unexpected error: {0}", ex2);
                }

                throw;
            }
        }

        private void ServiceConnectionCore()
        {
            byte[] messageBytes = StreamUtil.ReadFully(ClientStream);
            string messageString = Encoding.UTF8.GetString(messageBytes);

            // First deserialize as a non-specific Operation object to learn what operation it is.
            // Then we can deserialize as a more specific operation object once we know what operation it is.
            Operation operationCheck;
            try
            {
                operationCheck = JsonConvert.DeserializeObject<Operation>(messageString);
            }
            catch (JsonReaderException ex)
            {
                SendInvalidJsonError(ex);
                return;
            }

            if (operationCheck.OpName == null)
            {
                SendNoOpError();
                return;
            }

            if(!Operations.ContainsKey(operationCheck.OpName))
            {
                SendBadOpError(operationCheck.OpName);
                return;
            }

            OperationDescription opDescription = Operations[operationCheck.OpName];

            Operation derivedOp = (Operation)(JsonConvert.DeserializeObject(messageString, opDescription.OperationType));

            OperationReinterpreter opReinterpreter = new OperationReinterpreter(messageString);
            try
            {
                Response response = opDescription.OperationHandler(derivedOp, State, opReinterpreter);
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
            string responseJsonString = JsonConvert.SerializeObject(response);
            byte[] responseJsonBytes = Encoding.UTF8.GetBytes(responseJsonString);
            ClientStream.Write(responseJsonBytes, 0, responseJsonBytes.Length);
        }
    }
}

// Copyright (C) 2012 Greg Najda
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