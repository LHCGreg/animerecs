using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnimeRecs.MalApi
{
    /// <summary>
    /// This class is thread-safe if the underlying API is. If the expiration time is null, anime lists are cached for the lifetime of
    /// the object. Expired cache entries are only actually removed when a new anime list is inserted into the cache. Cache expiration
    /// measurement is susceptible to changes to the system clock.
    /// </summary>
    public class CachingMyAnimeListApi : IMyAnimeListApi
    {
        private IMyAnimeListApi m_underlyingApi;
        private bool m_ownUnderlyingApi;
        private MyAnimeListCache m_cache;

        public CachingMyAnimeListApi(IMyAnimeListApi underlyingApi, TimeSpan? expiration, bool ownApi = false)
        {
            m_underlyingApi = underlyingApi;
            m_ownUnderlyingApi = ownApi;
            m_cache = new MyAnimeListCache(expiration);
        }

        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            Logging.Log.InfoFormat("Checking cache for user {0}.", user);

            MalUserLookupResults animeList;
            if (m_cache.GetListForUser(user, out animeList))
            {
                if (animeList != null)
                {
                    Logging.Log.Info("Got anime list from cache.");
                    return animeList;
                }
                else
                {
                    // User does not have an anime list/no such user exists
                    Logging.Log.Info("Cache indicates that user does not have an anime list.");
                    throw new MalUserNotFoundException(string.Format("No MAL list exists for {0}.", user));
                }
            }
            else
            {
                Logging.Log.Info("Cache did not contain the anime list.");

                try
                {
                    animeList = m_underlyingApi.GetAnimeListForUser(user);
                    m_cache.PutListForUser(user, animeList);
                    return animeList;
                }
                catch (MalUserNotFoundException)
                {
                    // Cache the fact that the user does not have an anime list
                    m_cache.PutListForUser(user, null);
                    throw;
                }
            }
        }

        public RecentUsersResults GetRecentOnlineUsers()
        {
            return m_underlyingApi.GetRecentOnlineUsers();
        }

        public void Dispose()
        {
            m_cache.Dispose();
            if (m_ownUnderlyingApi)
            {
                m_underlyingApi.Dispose();
            }
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