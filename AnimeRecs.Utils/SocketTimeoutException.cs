using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.Utils
{
    public class SocketTimeoutException : Exception
    {
        public SocketTimeoutException() { }
        public SocketTimeoutException(string message) : base(message) { }
        public SocketTimeoutException(string message, Exception inner) : base(message, inner) { }
    }
}
