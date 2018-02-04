using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;

namespace AnimeRecs.DAL
{
    internal static class Logging
    {
        internal static ILog Log { get { return Common.Logging.LogManager.GetLogger("AnimeRecs.DAL"); } }
    }
}
