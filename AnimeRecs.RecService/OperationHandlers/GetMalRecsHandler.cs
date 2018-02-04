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
        public static async Task<Response> GetMalRecsAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<GetMalRecsRequest> operation = (Operation<GetMalRecsRequest>)baseOperation;
            return new Response<GetMalRecsResponse>() { Body = await state.GetMalRecsAsync(operation.Payload, cancellationToken).ConfigureAwait(false) };
        }
    }
}
