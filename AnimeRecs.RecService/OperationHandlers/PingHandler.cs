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
