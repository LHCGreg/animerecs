using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecService.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static async Task<Response> LoadRecSourceAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<LoadRecSourceRequest> operation = (Operation<LoadRecSourceRequest>)baseOperation;
            if (!operation.PayloadSet || operation.Payload == null)
                return GetArgumentNotSetError("Payload");
            if (operation.Payload.Name == null)
                return GetArgumentNotSetError("Payload.Name");
            if (operation.Payload.Type == null)
                return GetArgumentNotSetError("Payload.Type");

            try
            {
                await state.LoadRecSourceAsync(operation.Payload, cancellationToken).ConfigureAwait(false);
                return new Response();
            }
            catch (KeyNotFoundException)
            {
                return Response.GetErrorResponse(
                    errorCode: ErrorCodes.InvalidArgument,
                    message: string.Format("{0} is not a valid rec source type.", operation.Payload.Type)
                );
            }
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