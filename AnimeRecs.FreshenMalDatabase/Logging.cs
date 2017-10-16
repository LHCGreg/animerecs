using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging.NLog;
using System.Runtime.InteropServices;

namespace AnimeRecs.FreshenMalDatabase
{
    internal class Logging
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
                Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(new Common.Logging.Configuration.NameValueCollection()
                {
                    ["configType"] = "FILE",
                    ["configFile"] = loggingConfigPath
                });
            }
            Log = Common.Logging.LogManager.GetLogger("AnimeRecs.FreshenMalDatabase");
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

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.FreshenMalDatabase.
//
// AnimeRecs.FreshenMalDatabase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.FreshenMalDatabase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.FreshenMalDatabase.  If not, see <http://www.gnu.org/licenses/>.