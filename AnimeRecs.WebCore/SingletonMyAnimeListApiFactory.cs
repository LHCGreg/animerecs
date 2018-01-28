using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MalApi;

namespace AnimeRecs.WebCore
{
    class SingletonMyAnimeListApiFactory : IMyAnimeListApiFactory, IDisposable
    {
        private IMyAnimeListApi _api;

        public SingletonMyAnimeListApiFactory(IMyAnimeListApi api)
        {
            _api = api;
        }

        public IMyAnimeListApi GetMalApi()
        {
            return new NoDisposeMyAnimeListApi(_api);
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
