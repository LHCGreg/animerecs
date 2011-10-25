using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.UpdateCache
{
    internal class Logging
    {
        internal static global::Common.Logging.ILog Log { get; set; }

        internal static void SetUpLogging()
        {
            Log = global::Common.Logging.LogManager.GetLogger("AnimeRecs.UpdateCache");
            WriteLogPrologue();
            AnimeCompatibility.Logging.Initialize();
        }

        private static void WriteLogPrologue()
        {
            Logging.Log.InfoFormat("{0} started.", System.Reflection.Assembly.GetEntryAssembly().FullName);
            Logging.Log.DebugFormat("CLR Version: {0}", Environment.Version);
            Logging.Log.DebugFormat("Operating System: {0}", Environment.OSVersion);
            Logging.Log.DebugFormat("Number of processors: {0}", Environment.ProcessorCount);
        }
    }
}
