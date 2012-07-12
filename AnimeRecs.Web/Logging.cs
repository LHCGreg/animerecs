using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Web
{
    internal class Logging
    {
        internal static Common.Logging.ILog Log { get { return Common.Logging.LogManager.GetLogger("AnimeRecs.Web"); } }
    }
}