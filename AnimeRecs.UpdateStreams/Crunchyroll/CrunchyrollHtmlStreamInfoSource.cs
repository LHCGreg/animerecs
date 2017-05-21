using AnimeRecs.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                    fixedStreams[i] = new AnimeStreamInfo("THE IDOLM@STER CINDERELLA GIRLS Theater", "http://www.crunchyroll.com/the-idolmster-cinderella-girls-theater", StreamingService.Crunchyroll);
                }
            }

            return fixedStreams;
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.