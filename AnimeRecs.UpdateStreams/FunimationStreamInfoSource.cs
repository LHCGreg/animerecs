using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;
using HtmlAgilityPack;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    class FunimationStreamInfoSource : IAnimeStreamInfoSource
    {
        private IWebClient _webClient;

        public FunimationStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>();
            const string urlTemplate = "https://www.funimation.com/shows/all-shows/?sort=show&p={0}";
            for (int page = 1; ; page++)
            {
                if (page > 100)
                {
                    throw new Exception("Funimation has more pages of anime than expected, something is possibly broken.");
                }

                string url = string.Format(CultureInfo.InvariantCulture, urlTemplate, page);
                HtmlParsingAnimeStreamInfoSource helperSource = new HelperStreamInfoSource(_webClient, url);

                try
                {
                    ICollection<AnimeStreamInfo> streamsFromThisRequest = await helperSource.GetAnimeStreamInfoAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    streams.UnionWith(streamsFromThisRequest);
                }
                catch (NoMatchingHtmlException)
                {
                    if (streams.Count > 0)
                    {
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return streams;
        }

        internal class HelperStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            private static string xpath = "//div[contains(@class, 'product-list')]//div[contains(@class, 'show-wrapper')]/div[contains(@class, 'name')]/a";

            public HelperStreamInfoSource(IWebClient webClient, string url)
                : base(url, xpath, webClient)
            {

            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                string animeName = Utils.DecodeHtmlBody(matchingNode.InnerText);
                string url = Utils.DecodeHtmlAttribute(matchingNode.Attributes["href"].Value);
                return new AnimeStreamInfo(animeName, url, StreamingService.Funimation);
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
