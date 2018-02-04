using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class Response
    {
        public Error Error { get; set; }

        public static Response GetErrorResponse(string errorCode, string message)
        {
            return new Response()
            {
                Error = new Error(errorCode: errorCode, message: message)
            };
        }

        public Response()
        {
            ;
        }

        public Response(Error error)
        {
            Error = error;
        }
    }

    public class Response<TResponseBody> : Response
    {
        public TResponseBody Body { get; set; }
    }
}
