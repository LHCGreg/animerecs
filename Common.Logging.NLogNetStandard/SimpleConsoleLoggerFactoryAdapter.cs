using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Common.Logging.Configuration;

namespace AnimeRecs.CommonLoggingNetStandardStopgap
{
    public class SimpleConsoleLoggerFactoryAdapter : Common.Logging.Simple.AbstractSimpleLoggerFactoryAdapter
    {
        public SimpleConsoleLoggerFactoryAdapter()
            : base(null)
        {

        }

        public SimpleConsoleLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        {

        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            return new SimpleConsoleLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }
}
