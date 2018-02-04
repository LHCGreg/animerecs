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
        public static async Task<Response> ReloadTrainingDataAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<ReloadTrainingDataRequest> operation = (Operation<ReloadTrainingDataRequest>)baseOperation;
            ReloadBehavior behavior = (ReloadBehavior)Enum.Parse(typeof(ReloadBehavior), operation.Payload.Mode);
            await state.ReloadTrainingDataAsync(behavior, operation.Payload.Finalize, cancellationToken);
            return new Response();
        }
    }
}
