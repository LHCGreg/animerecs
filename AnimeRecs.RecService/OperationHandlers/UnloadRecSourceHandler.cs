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
        public static async Task<Response> UnloadRecSourceAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<UnloadRecSourceRequest> operation = (Operation<UnloadRecSourceRequest>)baseOperation;
            operation.Payload.AssertArgumentNotNull("Payload");
            operation.Payload.Name.AssertArgumentNotNull("Payload.Name");
            await state.UnloadRecSourceAsync(operation.Payload.Name, cancellationToken).ConfigureAwait(false);

            return new Response();
        }
    }
}
