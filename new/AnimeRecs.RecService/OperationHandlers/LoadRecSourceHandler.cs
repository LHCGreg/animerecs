using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.RecSources;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static Response LoadRecSource(Operation baseOperation, RecServiceState state, OperationReinterpreter opReinterpreter)
        {
            Operation<LoadRecSourceRequest<RecSourceParams>> operation = (Operation<LoadRecSourceRequest<RecSourceParams>>)baseOperation;
            if (!operation.PayloadSet || operation.Payload == null)
                return GetArgumentNotSetError("Payload");
            if (operation.Payload.Name == null)
                return GetArgumentNotSetError("Payload.Name");
            if (operation.Payload.Type == null)
                return GetArgumentNotSetError("Payload.Type");

            ITrainableJsonRecSource recSource;

            if (operation.Payload.Type.Equals(RecSourceTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                Operation<LoadRecSourceRequest<AverageScoreRecSourceParams>> opWithRecParams =
                    opReinterpreter.As<Operation<LoadRecSourceRequest<AverageScoreRecSourceParams>>>();
                recSource = new AverageScoreJsonRecSource(
                    minEpisodesToCountIncomplete: opWithRecParams.Payload.Params.MinEpisodesToCountIncomplete,
                    useDropped: opWithRecParams.Payload.Params.UseDropped,
                    minUsersToCountAnime: opWithRecParams.Payload.Params.MinUsersToCountAnime
                );
            }
            else if (operation.Payload.Type.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                Operation<LoadRecSourceRequest<MostPopularRecSourceParams>> opWithRecParams =
                    opReinterpreter.As<Operation<LoadRecSourceRequest<MostPopularRecSourceParams>>>();
                recSource = new MostPopularJsonRecSource(
                    minEpisodesToCountIncomplete: opWithRecParams.Payload.Params.MinEpisodesToCountIncomplete,
                    useDropped: opWithRecParams.Payload.Params.UseDropped
                );
            }
            else if (operation.Payload.Type.Equals(RecSourceTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
            {
                Operation<LoadRecSourceRequest<AnimeRecsRecSourceParams>> opWithRecParams =
                    opReinterpreter.As<Operation<LoadRecSourceRequest<AnimeRecsRecSourceParams>>>();

                throw new NotImplementedException("AnimeRecs handler not implemented yet."); // TODO
            }
            else
            {
                return Response.GetErrorResponse(
                    errorCode: ErrorCodes.InvalidArgument,
                    message: string.Format("{0} is not a valid rec source type.", operation.Payload.Type)
                );
            }

            try
            {
                state.LoadRecSource(recSource, operation.Payload.Name, operation.Payload.ReplaceExisting);
            }
            catch (RecServiceErrorException ex)
            {
                return new Response()
                {
                    Error = ex.Error
                };
            }

            return new Response();
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