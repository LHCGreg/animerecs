using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;
using AnimeRecs.Models;

namespace AnimeRecs
{
    public class SingleMyAnimeListApiFactory : IMyAnimeListApiFactory
    {
        private IMyAnimeListApi m_api;

        public SingleMyAnimeListApiFactory(IMyAnimeListApi api)
        {
            m_api = api;
        }

        public IMyAnimeListApi GetMalApi()
        {
            return new NoDisposeMalApi(m_api);
        }

        public void Dispose()
        {
            m_api.Dispose();
        }
    }
}