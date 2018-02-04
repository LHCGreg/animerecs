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
        public static async Task<Response> FinalizeRecSourcesAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<FinalizeRecSourcesRequest> operation = (Operation<FinalizeRecSourcesRequest>)baseOperation;
            await state.FinalizeRecSourcesAsync(cancellationToken);
            return new Response();
        }
    }
}
