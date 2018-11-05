using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;
using MiscUtil.Collections;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace AnimeRecs.UpdateStreams
{
    class HuluStreamInfoSource : IAnimeStreamInfoSource
    {
        private IWebClient _webClient;

        public HuluStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(streamInfo => streamInfo.Url, StringComparer.OrdinalIgnoreCase));

            int page = 1;
            while (true)
            {
                HuluStartPageStreamInfoSource helperSource = new HuluStartPageStreamInfoSource(page, _webClient);
                ICollection<AnimeStreamInfo> streamsFromPage = await helperSource.GetAnimeStreamInfoAsync(cancellationToken).ConfigureAwait(false);
                streams.UnionWith(streamsFromPage);

                if (page >= helperSource.NumberOfPages)
                {
                    break;
                }

                page++;
            }

            return streams.ToList();
        }

        internal class HuluStartPageStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            private static string Xpath = "//div[contains(@class, 'show-title-container')]/a";

            // Set after streams are extracted.
            public int NumberOfPages { get; private set; }

            public HuluStartPageStreamInfoSource(int page, IWebClient webClient)
                : base($"https://www.hulu.com/start/more_content?channel=anime&video_type=all&sort=alpha&is_current=0&closed_captioned=0&has_hd=0&page={page}", Xpath, webClient)
            {

            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                //<h3 style="margin:0px; width:150px;">
                //  <div class="play-button-hover"><a href="https://www.hulu.com/atelier-escha-and-logy-alchemists-of-the-dusk-sky" beaconid="23401" beacontype="show" class="beaconid beacontype info_hover force_plus " onclick="; Beacon.trackThumbnailClick(this);"><img alt="Atelier Escha &amp; Logy: Alchemists of the Dusk Sky" border="0" class="thumbnail" height="80" src="https://a248.e.akamai.net/ib.huluim.com/show_thumb/23401?size=145x80&amp;caller=h1o&amp;img=i.png" style="width: 145px; height: 80px; " title="" width="145" /></a></div>
                //</h3>
                //<div class="channel-results-show" style="clear:left;">
                //  <div class="show-title-container plus-only-show-title" style="width: 150px;">
                //    <a href="https://www.hulu.com/atelier-escha-and-logy-alchemists-of-the-dusk-sky" beaconid="23401" beacontype="show" class="beaconid beacontype force_plus info_hover" onclick="; Beacon.trackThumbnailClick(this);">Atelier Escha &amp; Logy: Alchemists...</a>
                //  </div>
                //  <div style="margin-top:0;">
                
                //      1 season |
                //      12 episodes
                
                //  </div>
                //</div>
                
                string url = Utils.DecodeHtmlAttribute(matchingNode.Attributes["href"].Value);

                HtmlNode tdNode = matchingNode.ParentNode.ParentNode.ParentNode;
                string imgXpathFromTd = ".//img[@class='thumbnail'][@alt]";
                HtmlNode imgNode = tdNode.SelectSingleNode(imgXpathFromTd);
                if (imgNode == null)
                {
                    throw new NoMatchingHtmlException($"Could not get anime name for link {url} on Hulu page {Url}. The site's HTML format probably changed.");
                }

                string animeName = Utils.DecodeHtmlAttribute(imgNode.Attributes["alt"].Value);

                return new AnimeStreamInfo(animeName, url, StreamingService.Hulu);
            }

            protected override void OnStreamsExtracted(HtmlDocument htmlDoc, List<AnimeStreamInfo> streams)
            {
                // Get the number of pages
                //       <a alt="Go to the last page" href="#" onclick="Pagination.loading($(this)); ; new Ajax.Updater('hp-more-content', '/start/more_content?closed_captioned=0&amp;amp;has_hd=0&amp;amp;is_current=0&amp;amp;page=17&amp;amp;sort=alpha', {asynchronous:true, evalScripts:true, method:'get', onComplete:function(request){Pagination.doneLoading($(this)); }}); return false;" title="Go to the last page">17</a>
                string lastPageLinkElementXpath = "//a[@title='Go to the last page']";
                HtmlNode matchingNode = htmlDoc.DocumentNode.SelectSingleNode(lastPageLinkElementXpath);

                if (matchingNode == null)
                {
                    throw new NoMatchingHtmlException(string.Format("Could not find the number of Hulu pages on {0}. The site's HTML format probably changed.", Url));
                }

                if (!int.TryParse(matchingNode.InnerText, out int numberOfPages))
                {
                    throw new NoMatchingHtmlException($"Was expecting an integer for Hulu number of pages on {Url}. Got \"{matchingNode.InnerText}\"");
                }

                NumberOfPages = numberOfPages;
            }
        }
    }
}
