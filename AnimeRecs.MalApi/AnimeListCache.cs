using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnimeRecs.MalApi
{
    /// <summary>
    /// Thread-safe cache with an optional expiration time. If the expiration time is null, anime lists are cached for the lifetime of
    /// the object. Expired cache entries are only actually removed when a new anime list is inserted into the cache. Cache expiration
    /// measurement is susceptible to changes to the system clock.
    /// </summary>
    internal class MyAnimeListCache
    {
        private Dictionary<string, MalUserLookupResults> m_animeListCache =
            new Dictionary<string, MalUserLookupResults>(StringComparer.InvariantCultureIgnoreCase);
        private LinkedList<Tuple<string, DateTime>> m_cachePutTimesSortedByTime;
        private Dictionary<string, LinkedListNode<Tuple<string, DateTime>>> m_cachePutTimesByName;
        private TimeSpan? m_expiration;

        private ReaderWriterLockSlim m_cacheLock = new ReaderWriterLockSlim();

        public MyAnimeListCache(TimeSpan? expiration)
        {
            m_expiration = expiration;
            if (m_expiration != null)
            {
                m_cachePutTimesSortedByTime = new LinkedList<Tuple<string, DateTime>>();
                m_cachePutTimesByName = new Dictionary<string, LinkedListNode<Tuple<string, DateTime>>>();
            }
        }

        public bool GetListForUser(string user, out MalUserLookupResults animeList)
        {
            m_cacheLock.EnterReadLock();

            try
            {
                if (m_expiration == null)
                {
                    if (m_animeListCache.TryGetValue(user, out animeList))
                    {
                        return true;
                    }
                    else
                    {
                        animeList = null;
                        return false;
                    }
                }

                LinkedListNode<Tuple<string, DateTime>> userAndTimeInsertedNode;

                // Check if this user is in the cache and if the cache entry is not stale
                if (m_cachePutTimesByName.TryGetValue(user, out userAndTimeInsertedNode))
                {
                    DateTime expirationTime = userAndTimeInsertedNode.Value.Item2 + m_expiration.Value;
                    if (DateTime.UtcNow < expirationTime)
                    {
                        animeList = m_animeListCache[user];
                        return true;
                    }
                    else
                    {
                        animeList = null;
                        return false;
                    }
                }
                else
                {
                    animeList = null;
                    return false;
                }
            }
            finally
            {
                m_cacheLock.ExitReadLock();
            }
        }

        public void PutListForUser(string user, MalUserLookupResults animeList)
        {
            m_cacheLock.EnterWriteLock();

            try
            {
                if (m_expiration == null)
                {
                    m_animeListCache[user] = animeList;
                    return;
                }

                LinkedListNode<Tuple<string, DateTime>> nodeForLastInsert;
                if (m_cachePutTimesByName.TryGetValue(user, out nodeForLastInsert))
                {
                    m_cachePutTimesSortedByTime.Remove(nodeForLastInsert);
                }

                DateTime nowUtc = DateTime.UtcNow;
                DateTime deleteOlderThan = nowUtc - m_expiration.Value;

                var newNode = m_cachePutTimesSortedByTime.AddFirst(new Tuple<string, DateTime>(user, nowUtc));

                m_cachePutTimesByName[user] = newNode;

                m_animeListCache[user] = animeList;

                // Check for old entries and remove them

                while (m_cachePutTimesSortedByTime.Count > 0 && m_cachePutTimesSortedByTime.Last.Value.Item2 < deleteOlderThan)
                {
                    string oldUser = m_cachePutTimesSortedByTime.Last.Value.Item1;
                    m_animeListCache.Remove(oldUser);
                    m_cachePutTimesByName.Remove(oldUser);
                    m_cachePutTimesSortedByTime.RemoveLast();
                }
            }
            finally
            {
                m_cacheLock.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            m_cacheLock.Dispose();
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