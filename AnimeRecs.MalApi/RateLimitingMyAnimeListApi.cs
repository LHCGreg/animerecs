using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace AnimeRecs.MalApi
{
    /// <summary>
    /// Limits MAL requests by waiting a given time period betwen each request.
    /// The time is measured from before one request is made to before the next request is made.
    /// You might use this to be nice to MAL and avoid making requests as fast as possible.
    /// This class is thread-safe if the underlying API is.
    /// </summary>
    public class RateLimitingMyAnimeListApi : IMyAnimeListApi
    {
        private IMyAnimeListApi m_underlyingApi;
        private bool m_ownApi;
        public TimeSpan TimeBetweenRequests { get; private set; }

        private Stopwatch m_stopwatchStartedAtLastRequest = null;
        private object m_syncHandle = new object();
        
        public RateLimitingMyAnimeListApi(IMyAnimeListApi underlyingApi, TimeSpan timeBetweenRequests, bool ownApi = false)
        {
            m_underlyingApi = underlyingApi;
            TimeBetweenRequests = timeBetweenRequests;
            m_ownApi = ownApi;
        }

        private void SleepIfNeededAndSetStopwatch()
        {
            lock (m_syncHandle)
            {
                if (m_stopwatchStartedAtLastRequest != null)
                {
                    TimeSpan timeSinceLastRequest = m_stopwatchStartedAtLastRequest.Elapsed;
                    if (timeSinceLastRequest < TimeBetweenRequests)
                    {
                        TimeSpan timeToWait = TimeBetweenRequests - timeSinceLastRequest;
                        Logging.Log.InfoFormat("Waiting {0} before making request.", timeToWait);
                        Thread.Sleep(timeToWait);
                    }
                }

                if (m_stopwatchStartedAtLastRequest == null)
                {
                    m_stopwatchStartedAtLastRequest = new Stopwatch();
                }

                m_stopwatchStartedAtLastRequest.Restart();
            }
        }
    
        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            SleepIfNeededAndSetStopwatch();
            return m_underlyingApi.GetAnimeListForUser(user);
        }

        public RecentUsersResults GetRecentOnlineUsers()
        {
            SleepIfNeededAndSetStopwatch();
            return m_underlyingApi.GetRecentOnlineUsers();
        }

        public void Dispose()
        {
            if (m_ownApi && m_underlyingApi != null)
            {
                m_underlyingApi.Dispose();
            }
        }
    }
}

/*
 Copyright 2012 Greg Najda

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