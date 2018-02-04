using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class Error
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        public Error()
        {
            ;
        }

        public Error(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
