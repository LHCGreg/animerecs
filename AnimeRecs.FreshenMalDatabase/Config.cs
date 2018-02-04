using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.FreshenMalDatabase
{
    class Config
    {
        public ConfigConnectionStrings ConnectionStrings { get; set; }

        public int UsersPerRun { get; set; }
        public int MaxUsersInDatabase { get; set; }
        public int DelayBetweenRequestsInMs { get; set; }
        public int MalTimeoutInMs { get; set; }
        public string MalApiUserAgentString { get; set; }
        public int MinimumAnimesCompletedAndRated { get; set; }
        public int NumMalRequestFailuresBeforeGivingUp { get; set; }
        public int DelayAfterMalRequestFailureInMs { get; set; }
        public string LoggingConfigPath { get; set; }

        public class ConfigConnectionStrings
        {
            public string AnimeRecs { get; set; }
        }
    }
}
