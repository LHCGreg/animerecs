using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AnimeRecs.UpdateStreams
{
    abstract class HtmlParsingAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        protected IWebClient WebClient { get; private set; }
        protected string Url { get; private set; }
        protected string XPath { get; private set; }

        protected HtmlParsingAnimeStreamInfoSource(string url, string xpath, IWebClient webClient)
        {
            Url = url;
            XPath = xpath;
            WebClient = webClient;
        }

        /// <summary>
        /// Subclasses can add headers here.
        /// </summary>
        protected virtual void ModifyRequestBeforeSending(WebClientRequest request)
        {

        }
        
        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting HTML for {0}", Url);
            WebClientRequest request = new WebClientRequest(Url);
            ModifyRequestBeforeSending(request);

            string responseBody = await WebClient.GetStringAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNodeCollection matchingNodes = htmlDoc.DocumentNode.SelectNodes(XPath);

            if (matchingNodes == null || matchingNodes.Count == 0)
            {
                throw new NoMatchingHtmlException(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
            }

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            foreach (HtmlNode matchingNode in matchingNodes)
            {
                AnimeStreamInfo stream = GetStreamInfoFromMatch(matchingNode);
                
                // Convert possibly relative url to an absolute url
                stream = new AnimeStreamInfo(stream.AnimeName, Utils.PossiblyRelativeUrlToAbsoluteUrl(stream.Url, Url), stream.Service);
                
                streams.Add(stream);
            }

            OnStreamsExtracted(htmlDoc, streams);

            return streams;
        }

        protected virtual void OnStreamsExtracted(HtmlDocument htmlDoc, List<AnimeStreamInfo> streams)
        {

        }

        protected abstract AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode);
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
