using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MalApi;

namespace AnimeRecs.Web
{
    public interface IMyAnimeListApiFactory
    {
        IMyAnimeListApi GetMalApi();
    }
}
