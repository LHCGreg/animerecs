using AnimeRecs.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    class AmazonPrimeStreamInfoSource : AmazonStreamInfoSource
    {
        private const string FirstPageUrl = "https://www.amazon.com/s/ref=sr_rot_p_n_ways_to_watch_1?rh=n:2858778011,p_n_theme_browse-bin:2650364011,p_n_ways_to_watch:12007865011&ie=UTF8";

        public AmazonPrimeStreamInfoSource(IWebClient webClient)
            : base(FirstPageUrl, StreamingService.AmazonPrime, webClient)
        {

        }
    }
}
