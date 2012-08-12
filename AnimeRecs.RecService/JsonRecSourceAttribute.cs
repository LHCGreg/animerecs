using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class JsonRecSourceAttribute : Attribute
    {
        public string RecSourceName { get; private set; }

        public JsonRecSourceAttribute(string recSourceName)
        {
            RecSourceName = recSourceName;
        }
    }
}
