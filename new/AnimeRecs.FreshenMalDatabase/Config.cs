using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace AnimeRecs.FreshenMalDatabase
{
    class Config
    {
        public int UsersPerRun { get; private set; }
        public int MaxUsersInDatabase { get; private set; }
        public int DelayBetweenRequestsInMs { get; private set; }
        public int MalTimeoutInMs { get; private set; }
        public string MalApiUserAgentString { get; private set; }
        public int MinimumAnimesCompletedAndRated { get; private set; }
        public int NumMalRequestFailuresBeforeGivingUp { get; private set; }
        public int DelayAfterMalRequestFailureInMs { get; private set; }

        public string PostgresConnectionString { get; private set; }

        public Config()
        {
            UsersPerRun = int.Parse(ConfigurationManager.AppSettings["UsersPerRun"]);
            MaxUsersInDatabase = int.Parse(ConfigurationManager.AppSettings["MaxUsersInDatabase"]);
            DelayBetweenRequestsInMs = int.Parse(ConfigurationManager.AppSettings["DelayBetweenRequestsMs"]);
            MalTimeoutInMs = int.Parse(ConfigurationManager.AppSettings["MalTimeoutInMs"]);
            MalApiUserAgentString = ConfigurationManager.AppSettings["MalApiUserAgentString"];
            MinimumAnimesCompletedAndRated = int.Parse(ConfigurationManager.AppSettings["MinimumAnimesCompletedAndRated"]);
            NumMalRequestFailuresBeforeGivingUp = int.Parse(ConfigurationManager.AppSettings["NumMalRequestFailuresBeforeGivingUp"]);
            DelayAfterMalRequestFailureInMs = int.Parse(ConfigurationManager.AppSettings["DelayAfterMalRequestFailureInMs"]);
            PostgresConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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