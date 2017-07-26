using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static Task<Response> PingAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<PingRequest> operation = (Operation<PingRequest>)(baseOperation);
            if (!operation.PayloadSet || operation.Payload == null)
                return Task.FromResult(GetArgumentNotSetError("Payload"));
            if (operation.Payload.PingMessage == null)
                return Task.FromResult(GetArgumentNotSetError("Payload.PingMessage"));

            return Task.FromResult<Response>(new Response<PingResponse>()
            {
                Body = new PingResponse()
                {
                    OriginalMessage = operation.Payload.PingMessage,
                    ResponseMessage = string.Format("Your message was \"{0}\".", operation.Payload.PingMessage)
                }
            });
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