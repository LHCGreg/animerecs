using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class NoConcurrentFetchesOfSameUserMalApi : IMyAnimeListApi
    {
        private IMyAnimeListApi m_underlyingApi;
        private HashSet<string> m_usersCurrentlyBeingLookedUp = new HashSet<string>();
        private const int MillisecondsToSleep = 100;
        
        public NoConcurrentFetchesOfSameUserMalApi(IMyAnimeListApi underlyingApi)
        {
            m_underlyingApi = underlyingApi;
        }

        public ICollection<MyAnimeListEntry> GetAnimeListForUser(string user)
        {
            while (true)
            {
                bool currentlyBeingLookedUp = false;
                lock (m_usersCurrentlyBeingLookedUp)
                {
                    currentlyBeingLookedUp = m_usersCurrentlyBeingLookedUp.Contains(user);
                    if (!currentlyBeingLookedUp)
                    {
                        m_usersCurrentlyBeingLookedUp.Add(user);
                    }
                }

                if (!currentlyBeingLookedUp)
                {
                    try
                    {
                        return m_underlyingApi.GetAnimeListForUser(user);
                    }
                    finally
                    {
                        m_usersCurrentlyBeingLookedUp.Remove(user);
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(MillisecondsToSleep);
                }
            }
        }

        public void Dispose()
        {
            m_underlyingApi.Dispose();
        }
    }
}