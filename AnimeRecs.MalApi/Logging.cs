using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;

namespace AnimeRecs.MalApi
{
    public static class Logging
    {
        private static ILog s_log = new NoOpLogger();

        /// <summary>
        /// Gets or sets the logger that the AnimeCompatibility library should use. Logging can ease
        /// troubleshooting. Setting this property to null is shorthand for setting it to a
        /// Common.Logging.Simple.NoOpLogger. If a logger is never set, a Common.Logging.Simple.NoOpLogger
        /// is used.
        /// </summary>
        internal static ILog Log
        {
            get
            {
                return s_log;
            }
            set
            {
                if (value == null)
                {
                    s_log = new NoOpLogger();
                }
                else
                {
                    s_log = value;
                }
            }
        }

        public static void Initialize()
        {
            Log = Common.Logging.LogManager.GetLogger("MAL API");
        }
    }
}

/*
 Copyright 2011 Greg Najda

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/