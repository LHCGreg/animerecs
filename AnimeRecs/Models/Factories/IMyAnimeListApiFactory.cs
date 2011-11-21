using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeCompatibility;

namespace AnimeRecs
{
    public interface IMyAnimeListApiFactory : IDisposable
    {
        IMyAnimeListApi GetMalApi();
    }
}
