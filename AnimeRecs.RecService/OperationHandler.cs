using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService
{
    internal delegate Task<Response> OperationHandler(Operation baseOperation, RecServiceState state, CancellationToken cancellationToken);
}
