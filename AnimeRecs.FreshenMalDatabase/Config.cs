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