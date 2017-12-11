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
