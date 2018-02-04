using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService
{
    internal static class ValidationExtensions
    {
        public static void AssertArgumentNotNull<T>(this T arg, string argPath)
            where T : class
        {
            if (arg == null)
                throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.InvalidArgument, message: string.Format("{0} was not set.", argPath)));
        }
    }
}
