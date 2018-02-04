using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.RecService.DTO
{
    /// <summary>
    /// Used to mark a class as used for JSON serializing/deserializing. This can be used for preserializing all JSON classes
    /// in long-running processes like servers to speed up the first use of a class for serializing/deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class JsonClassAttribute : Attribute
    {
        public JsonClassAttribute()
        {

        }
    }
}
