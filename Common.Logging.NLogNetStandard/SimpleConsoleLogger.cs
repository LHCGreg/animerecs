using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace AnimeRecs.CommonLoggingNetStandardStopgap
{
    class SimpleConsoleLogger : AbstractSimpleLogger
    {
        public SimpleConsoleLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            FormatOutput(sb, level, message, exception);
            Console.Out.WriteLine(sb.ToString());
        }
    }
}
