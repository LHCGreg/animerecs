using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        internal static Response GetArgumentNotSetError(string argPath)
        {
            return Response.GetErrorResponse(errorCode: ErrorCodes.InvalidArgument, message: string.Format("{0} was not set.", argPath));
        }
    }
}
