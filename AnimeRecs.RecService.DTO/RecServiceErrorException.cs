using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public class RecServiceErrorException : Exception
    {
        public Error Error { get; private set; }

        public RecServiceErrorException(Error error)
            : base(error.Message)
        {
            Error = error;
        }

        public RecServiceErrorException(Error error, Exception innerException)
            : base(error.Message, innerException)
        {
            Error = error;
        }
    }
}
