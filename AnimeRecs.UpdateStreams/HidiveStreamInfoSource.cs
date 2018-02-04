using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using HtmlAgilityPack;
using MiscUtil.Collections;

namespace AnimeRecs.UpdateStreams
{
    class HidiveStreamInfoSource : IAnimeStreamInfoSource
    {
        IWebClient _webClient;

        public HidiveStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(streamInfo => streamInfo.Url, StringComparer.OrdinalIgnoreCase));

            HelperStreamInfoSource tvStreamSource = new HelperStreamInfoSource("https://www.hidive.com/tv", _webClient);
            ICollection<AnimeStreamInfo> tvAnime = await tvStreamSource.GetAnimeStreamInfoAsync(cancellationToken).ConfigureAwait(false);
            streams.UnionWith(tvAnime);

            HelperStreamInfoSource movieAndOvaStreamSource = new HelperStreamInfoSource("https://www.hidive.com/movies", _webClient);
            ICollection<AnimeStreamInfo> movieAndOvaAnime = await movieAndOvaStreamSource.GetAnimeStreamInfoAsync(cancellationToken).ConfigureAwait(false);
            streams.UnionWith(movieAndOvaAnime);

            return streams.ToList();
        }

        internal class HelperStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            const string StreamRootXpath = "//div[contains(@class, 'hitbox')]";

            public HelperStreamInfoSource(string url, IWebClient webClient)
                : base(url, StreamRootXpath, webClient)
            {

            }

            protected override void ModifyRequestBeforeSending(WebClientRequest request)
            {
                // Hidive returns a 403 forbidden if we don't use a browser's user agent.
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0";
            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                HtmlNode urlNode = matchingNode.SelectSingleNode(".//div[contains(@class, 'player')]/a");
                if (urlNode == null)
                {
                    throw new NoMatchingHtmlException(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
                }
                string possiblyRelativeUrl = Utils.DecodeHtmlAttribute(urlNode.Attributes["href"].Value);

                HtmlNode titleNode = matchingNode.SelectSingleNode(".//div[contains(@class, 'synopsis')]/h3");
                if (titleNode == null)
                {
                    throw new NoMatchingHtmlException(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
                }
                string animeName = Utils.DecodeHtmlBody(titleNode.InnerText);

                return new AnimeStreamInfo(animeName, possiblyRelativeUrl, StreamingService.Hidive);
            }
        }
    }
}

