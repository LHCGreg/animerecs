using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static Response LoadRecSource(Operation baseOperation, RecServiceState state)
        {
            Operation<LoadRecSourceRequest> operation = (Operation<LoadRecSourceRequest>)baseOperation;
            if (!operation.PayloadSet || operation.Payload == null)
                return GetArgumentNotSetError("Payload");
            if (operation.Payload.Name == null)
                return GetArgumentNotSetError("Payload.Name");
            if (operation.Payload.Type == null)
                return GetArgumentNotSetError("Payload.Type");

            if (state.JsonRecSourceTypes.ContainsKey(operation.Payload.Type))
            {
                Type jsonRecSourceType = state.JsonRecSourceTypes[operation.Payload.Type];

                // operation.Payload's static type is LoadRecSourceRequest.
                // Its real type will be something like LoadRecSourceRequest<AverageScoreRecSourceParams> thanks to the custom JsonConverter.
                // A json rec source is expected to have one or more constructors taking types derived from LoadRecSourceRequest.
                Func<ITrainableJsonRecSource> recSourceFactory = () => (ITrainableJsonRecSource)(Activator.CreateInstance(jsonRecSourceType, operation.Payload));

                state.LoadRecSource(recSourceFactory, operation.Payload.Name, operation.Payload.ReplaceExisting);
                return new Response();
            }
            else
            {
                return Response.GetErrorResponse(
                    errorCode: ErrorCodes.InvalidArgument,
                    message: string.Format("{0} is not a valid rec source type.", operation.Payload.Type)
                );
            }
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