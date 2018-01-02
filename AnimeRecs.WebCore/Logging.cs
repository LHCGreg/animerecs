using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Common.Logging.NLog;
using NLog.Web;

namespace AnimeRecs.WebCore
{
    internal static class Logging
    {
        internal static global::Common.Logging.ILog Log { get; set; }

        internal static void SetUpLogging(string loggingConfigPath)
        {
            if (loggingConfigPath == null)
            {
                Common.Logging.LogManager.Adapter = new AnimeRecs.CommonLoggingNetStandardStopgap.SimpleConsoleLoggerFactoryAdapter();
            }
            else
            {
                NLogBuilder.ConfigureNLog(loggingConfigPath);

                // Do not pass the log config file here, because NLog has already been configured with NLogBuilder.ConfigureNLog.
                Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(new Common.Logging.Configuration.NameValueCollection());
            }
            Log = Common.Logging.LogManager.GetLogger("AnimeRecs.Web");
            WriteLogPrologue();
        }

        internal static void SetUpConsoleLogging()
        {
            SetUpLogging(loggingConfigPath: null);
        }

        private static void WriteLogPrologue()
        {
            Logging.Log.InfoFormat("{0} started.", System.Reflection.Assembly.GetEntryAssembly().FullName);
            Logging.Log.DebugFormat("CLR Version: {0}, Operating System: {1}, OS Architecture: {2}, Processor Architecture: {3}, Number of Processors: {4}",
                RuntimeInformation.FrameworkDescription, RuntimeInformation.OSDescription, RuntimeInformation.OSArchitecture, RuntimeInformation.ProcessArchitecture, Environment.ProcessorCount);
        }
    }
}
