using AnimeRecs.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace AnimeRecs.UpdateStreams.Crunchyroll
{
    /// <summary>
    /// Takes in Crunchyroll html from http://www.crunchyroll.com/videos/anime/alpha?group=all
    /// and returns stream info.
    /// </summary>
    class CrunchyrollHtmlStreamInfoSource : IAnimeStreamInfoSource
    {
        internal const string AnimeListUrl = "http://www.crunchyroll.com/videos/anime/alpha?group=all";
        private const string AnimeRegexString =
    "<a title=\"[^\"]*?\" token=\"shows-portraits\" itemprop=\"url\" href=\"(?<Url>[^\"]*?)\" [^>]*?>\\s*(?<AnimeName>.*?)\\s*?</a>";

        private static Lazy<Regex> AnimeRegex = new Lazy<Regex>(
            () => new Regex(AnimeRegexString, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
            isThreadSafe: true);

        private string _html;

        public CrunchyrollHtmlStreamInfoSource(string html)
        {
            _html = html;
        }

        public Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            // We're not making any http requests in this class and it's not especially CPU intensive, so just run synchronously
            // and wrap the result in a task.
            return Task.FromResult(GetAnimeStreamInfo());
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            HtmlRegexAnimeStreamInfoSource regexSource = new HtmlRegexAnimeStreamInfoSource(AnimeListUrl,
                AnimeRegex.Value, _html, StreamingService.Crunchyroll,
                animeNameContext: HtmlRegexContext.Body, urlContext: HtmlRegexContext.Attribute);

            ICollection<AnimeStreamInfo> streams = regexSource.GetAnimeStreamInfo();
            List<AnimeStreamInfo> fixedStreams = new List<AnimeStreamInfo>(streams);
            for (int i = 0; i < fixedStreams.Count; i++)
            {
                // Hack to work around Crunchyroll having this where "IDOLM@STER" would normally be:
                // <span class=""__cf_email__"" data-cfemail=""8fc6cbc0c3c2cfdcdbcadd"">[email protected]</span><script data-cfhash='f9e31' type=""text/javascript"">/* <![CDATA[ */!function(t,e,r,n,c,a,p){try{t=document.currentScript||function(){for(t=document.getElementsByTagName('script'),e=t.length;e--;)if(t[e].getAttribute('data-cfhash'))return t[e]}();if(t&&(c=t.previousSibling)){p=t.parentNode;if(a=c.getAttribute('data-cfemail')){for(e='',r='0x'+a.substr(0,2)|0,n=2;a.length-n;n+=2)e+='%'+('0'+('0x'+a.substr(n,2)^r).toString(16)).slice(-2);p.replaceChild(document.createTextNode(decodeURIComponent(e)),c)}p.removeChild(t)}}catch(u){}}()/* ]]> */</script>
                if (fixedStreams[i].Url == "http://www.crunchyroll.com/the-idolmster-cinderella-girls-theater")
                {
                    fixedStreams[i] = new AnimeStreamInfo("THE IDOLM@STER CINDERELLA GIRLS Theater", fixedStreams[i].Url, StreamingService.Crunchyroll);
                }
                if (fixedStreams[i].Url == "http://www.crunchyroll.com/the-idolmster-side-m")
                {
                    fixedStreams[i] = new AnimeStreamInfo("THE IDOLM@STER Side M", fixedStreams[i].Url, StreamingService.Crunchyroll);
                }
                if (fixedStreams[i].Url == "http://www.crunchyroll.com/the-idolmster-cinderella-girls")
                {
                    fixedStreams[i] = new AnimeStreamInfo("THE IDOLM@STER CINDERELLA GIRLS", fixedStreams[i].Url, StreamingService.Crunchyroll);
                }
                if (fixedStreams[i].Url == "http://www.crunchyroll.com/puchims")
                {
                    fixedStreams[i] = new AnimeStreamInfo("PUCHIM@S", fixedStreams[i].Url, StreamingService.Crunchyroll);
                }
                if (fixedStreams[i].Url == "http://www.crunchyroll.com/the-idolmster-sidem-wakeatte-mini")
                {
                    fixedStreams[i] = new AnimeStreamInfo("THE IDOLM@STER SideM Wakeatte Mini!", fixedStreams[i].Url, StreamingService.Crunchyroll);
                }
            }

            return fixedStreams;
        }
    }
}
