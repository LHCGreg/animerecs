using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace AnimeRecs.RecService.ClientLib
{
    internal static class Logging
    {
        internal static ILog Log { get { return Common.Logging.LogManager.GetLogger("AnimeRecs.RecService.ClientLib"); } }
    }
}