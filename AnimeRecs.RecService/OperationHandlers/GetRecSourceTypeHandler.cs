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
        public static async Task<Response> GetRecSourceTypeAsync(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken)
        {
            Operation<GetRecSourceTypeRequest> operation = (Operation<GetRecSourceTypeRequest>)baseOperation;
            string recSourceType = await state.GetRecSourceTypeAsync(operation.Payload.RecSourceName, cancellationToken).ConfigureAwait(false);
            return new Response<GetRecSourceTypeResponse>()
            {
                Body = new GetRecSourceTypeResponse(recSourceType)
            };
        }
    }
}
