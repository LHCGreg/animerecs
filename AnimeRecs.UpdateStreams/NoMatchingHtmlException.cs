using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    public class NoMatchingHtmlException : Exception
    {
        public NoMatchingHtmlException() { }
        public NoMatchingHtmlException(string message) : base(message) { }
        public NoMatchingHtmlException(string message, Exception inner) : base(message, inner) { }
    }
}
