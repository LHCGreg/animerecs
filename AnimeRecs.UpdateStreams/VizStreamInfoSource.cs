using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class VizStreamInfoSource : HtmlParsingAnimeStreamInfoSource
    {
        private const string URL = "http://www.viz.com/watch/streaming/watch";
        private const string AnimeDivXPath = @"//a[contains(@class,'o_property-link')]";

        public VizStreamInfoSource(IWebClient webClient)
            : base(URL, AnimeDivXPath, webClient)
        {
            ;
        }

        protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
        {
            string possiblyRelativeUrl = Utils.DecodeHtmlAttribute(matchingNode.Attributes["href"].Value);

            HtmlNode animeNameDiv = matchingNode.ChildNodes.Where(node => node.NodeType == HtmlNodeType.Element && node.Name == "div" && node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("type-center")).FirstOrDefault();
            if (animeNameDiv == null)
            {
                throw new Exception(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
            }

            string animeName = Utils.DecodeHtmlBody(animeNameDiv.InnerText);
            return new AnimeStreamInfo(animeName, possiblyRelativeUrl, StreamingService.Viz);
        }
    }
}
