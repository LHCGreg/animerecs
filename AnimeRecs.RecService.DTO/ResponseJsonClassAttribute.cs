using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    /// <summary>
    /// Used to mark a class as used as the T in Response&lt;T&gt;. This can be used for preserializing all JSON classes
    /// in long-running processes like servers to speed up the first use of a class for serializing/deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class ResponseJsonClassAttribute : Attribute
    {
        public ResponseJsonClassAttribute()
        {

        }
    }
}
