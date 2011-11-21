using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    /// <summary>
    /// Wraps another IMyAnimeListApi and does not dispose of the other api.
    /// </summary>
    public class NoDisposeMalApi : IMyAnimeListApi
    {
        private IMyAnimeListApi m_underlyingApi;

        public NoDisposeMalApi(IMyAnimeListApi underlyingApi)
        {
            m_underlyingApi = underlyingApi;
        }

        public ICollection<MyAnimeListEntry> GetAnimeListForUser(string user)
        {
            return m_underlyingApi.GetAnimeListForUser(user);
        }

        public void Dispose()
        {
            ;
        }
    }
}