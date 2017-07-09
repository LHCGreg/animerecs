using MiscUtil.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using AnimeRecs.DAL;
using System.Text.RegularExpressions;

namespace AnimeRecs.UpdateStreams
{
    abstract class AmazonStreamInfoSource : IAnimeStreamInfoSource
    {
        private string _firstPageUrl;
        private StreamingService _service;
        private IWebClient _webClient;

        protected AmazonStreamInfoSource(string firstPageUrl, StreamingService service, IWebClient webClient)
        {
            _firstPageUrl = firstPageUrl;
            _service = service;
            _webClient = webClient;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            // Get first page
            // Parse out streams
            // Parse out next page URL
            // If next page URL exists, get next page, repeat
            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(streamInfo => streamInfo.Url, StringComparer.OrdinalIgnoreCase));
            string nextPageUrl = _firstPageUrl;
            do
            {
                ParsedPageResults results = GetPageResults(nextPageUrl);
                streams.UnionWith(results.Streams);
                nextPageUrl = results.NextPageUrl;
            } while (nextPageUrl != null);

            return streams;
        }

        private class ParsedPageResults
        {
            public ICollection<AnimeStreamInfo> Streams { get; private set; }
            public string NextPageUrl { get; private set; }

            public ParsedPageResults(ICollection<AnimeStreamInfo> streams, string nextPageUrl)
            {
                Streams = streams;
                NextPageUrl = nextPageUrl;
            }
        }

        private ParsedPageResults GetPageResults(string url)
        {
            AmazonPageStreamInfoSource pageSource = new AmazonPageStreamInfoSource(url, _service, _webClient);
            ICollection<AnimeStreamInfo> streams = pageSource.GetAnimeStreamInfo();

            return new ParsedPageResults(streams: streams, nextPageUrl: pageSource.NextPageUrl);
        }

        private static Regex s_qidRegex = new Regex(@"qid=\d+&?", RegexOptions.Compiled);

        private class AmazonPageStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            // <a class="a-link-normal s-access-detail-page  s-color-twister-title-link a-text-normal" title="Grimoire of Zero - Season 1" href="https://www.amazon.com/Thirteen/dp/B06Y3CSGF1/ref=sr_1_2/143-2912789-5885700?s=instant-video&amp;ie=UTF8&amp;qid=1495391820&amp;sr=1-2&amp;refinements=p_n_ways_to_watch%3A12007866011%2Cp_n_subscription_id%3A16182082011">
		       // <h2 data-attribute="Grimoire of Zero - Season 1" data-max-rows="0" data-truncate-by-character="false" class="a-size-medium s-inline  s-access-title  a-text-normal">Grimoire of Zero - Season 1</h2>
	        // </a>
            private const string AnimeXPath = "//a[contains(@class, 's-access-detail-page')]/h2";
            private const string NextPageLinkXPath = "//a[@id='pagnNextLink']";

            public string NextPageUrl { get; private set; }

            private StreamingService _service;

            public AmazonPageStreamInfoSource(string url, StreamingService service, IWebClient webClient)
                : base(url, AnimeXPath, webClient)
            {
                _service = service;
            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                string animeName = Utils.DecodeHtmlBody(matchingNode.InnerText);
                HtmlNode link = matchingNode.ParentNode;
                string url = Utils.DecodeHtmlAttribute(link.Attributes["href"].Value);

                // https://www.amazon.com/Acceleration/dp/B00GQ1HB3Q/ref=sr_1_37/141-4748842-3456620?s=instant-video&ie=UTF8&sr=1-37&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011
                // to
                // https://www.amazon.com/Acceleration/dp/B00GQ1HB3Q
                //
                // All the stuff after is unncessary and some of it changes on every search.
                // The qid and the 141-4748842-3456620 definitely changes.
                // ref=sr_1_37 and sr=1-37 probably can change if the position of the anime in the search results changes.

                int indexOfUnnecessaryUrlStuff = url.IndexOf("/ref=");
                if (indexOfUnnecessaryUrlStuff == -1)
                {
                    throw new Exception(string.Format("Did not find /ref= in Amazon url. The URL format may have changed. The URL was: {0}", url));
                }

                url = url.Substring(0, indexOfUnnecessaryUrlStuff);
                
                return new AnimeStreamInfo(animeName, url, _service);
            }

            protected override void OnStreamsExtracted(HtmlDocument htmlDoc, List<AnimeStreamInfo> streams)
            {
                // Set NextPageUrl

                HtmlNode nextPageLinkNode = htmlDoc.DocumentNode.SelectSingleNode(NextPageLinkXPath);
                if (nextPageLinkNode == null)
                {
                    NextPageUrl = null;
                }
                else
                {
                    string possiblyRelativeNextPageUrl = Utils.DecodeHtmlAttribute(nextPageLinkNode.Attributes["href"].Value);
                    NextPageUrl = Utils.PossiblyRelativeUrlToAbsoluteUrl(possiblyRelativeNextPageUrl, Url);
                }
            }
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
