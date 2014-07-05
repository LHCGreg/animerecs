using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService
{
    internal class Logging
    {
        internal static global::Common.Logging.ILog Log { get; set; }

        internal static void SetUpLogging()
        {
            Log = global::Common.Logging.LogManager.GetLogger("AnimeRecs.RecService");
            WriteLogPrologue();
        }

        private static void WriteLogPrologue()
        {
            Logging.Log.DebugFormat("{0} started.", System.Reflection.Assembly.GetEntryAssembly().FullName);
            Logging.Log.DebugFormat("CLR Version: {0}", Environment.Version);
            Logging.Log.DebugFormat("Operating System: {0}", Environment.OSVersion);
            Logging.Log.DebugFormat("Number of processors: {0}", Environment.ProcessorCount);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.