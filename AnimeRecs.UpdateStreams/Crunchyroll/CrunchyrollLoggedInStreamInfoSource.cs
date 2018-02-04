using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams.Crunchyroll
{
    /// <summary>
    /// Using the supplied cookies obtained from logging in, gets stream info from crunchyroll's website.
    /// </summary>
    internal class CrunchyrollLoggedInStreamInfoSource : WebPageStreamInfoSource
    {
        public CrunchyrollLoggedInStreamInfoSource(IWebClient loggedInWebClient)
            : base(CrunchyrollHtmlStreamInfoSource.AnimeListUrl, loggedInWebClient)
        {
            
        }

        protected override ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string html)
        {
            var source = new CrunchyrollHtmlStreamInfoSource(html);
            return source.GetAnimeStreamInfo();
        }
    }
}
