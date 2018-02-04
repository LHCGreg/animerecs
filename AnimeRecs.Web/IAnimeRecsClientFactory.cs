using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.RecService.ClientLib;

namespace AnimeRecs.Web
{
    public interface IAnimeRecsClientFactory
    {
        AnimeRecsClient GetClient(string recSourceName);
    }
}
