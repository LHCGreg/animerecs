using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AnimeRecs.WebCore
{
    /// <summary>
    /// Used to return a response from controller method early, from deeper in the call stack.
    /// </summary>
    class ShortCircuitException : Exception
    {
        public IActionResult Result { get; private set; }

        public ShortCircuitException(IActionResult result)
        {
            Result = result;
        }
    }
}
