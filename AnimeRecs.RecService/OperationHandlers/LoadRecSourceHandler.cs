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
