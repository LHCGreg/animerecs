using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using HtmlAgilityPack;
using MiscUtil.Collections;

namespace AnimeRecs.UpdateStreams
{
    class AnimeNetworkStreamInfoSource : IAnimeStreamInfoSource
    {
        private IWebClient _webClient;

        public AnimeNetworkStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            List<string> letters = new List<string>(27);
            letters.Add("0");
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            HashSet<AnimeStreamInfo> streamsFromAllPages = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(streamInfo => streamInfo.Url, StringComparer.OrdinalIgnoreCase));

            foreach (string letter in letters)
            {
                // Get http://www.theanimenetwork.com/Watch-Anime/Alphabet/<letter>

                // Look for
                // <div class="col-lg-3 col-md-4 col-sm-6 text-center titleimg">
                //    <h3 class="small hidden-sm hidden-xs">A-Channel</h3>
                //    <a href="/Watch-Anime/A-Channel">
                string url = string.Format("http://www.theanimenetwork.com/Watch-Anime/Alphabet/{0}", letter);
                AnimeNetworkPageStreamInfoSource pageSource = new AnimeNetworkPageStreamInfoSource(url, _webClient);

                try
                {
                    ICollection<AnimeStreamInfo> streamsFromThisPage = pageSource.GetAnimeStreamInfo();
                    streamsFromAllPages.UnionWith(streamsFromThisPage);
                }
                catch (NoMatchingHtmlException)
                {
                    // It can happen for no anime to begin with a letter, or for the "0" page (labeled as "#") for no anime to begin with a number or symbol.
                    continue;
                }

            }

            return streamsFromAllPages;
        }

        private class AnimeNetworkPageStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            private const string AnimeDivXPath = @"//div[contains(@class,'titleimg')]";

            public AnimeNetworkPageStreamInfoSource(string url, IWebClient webClient)
                : base(url, AnimeDivXPath, webClient)
            {

            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                // Look for
                // <div class="col-lg-3 col-md-4 col-sm-6 text-center titleimg">
                //    <h3 class="small hidden-sm hidden-xs">A-Channel</h3>
                //    <a href="/Watch-Anime/A-Channel">

                HtmlNode animeNameTag = matchingNode.ChildNodes.Where(node => node.NodeType == HtmlNodeType.Element && node.Name == "h3").FirstOrDefault();
                if (animeNameTag == null)
                {
                    throw new Exception(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
                }

                HtmlNode animeLinkTag = matchingNode.ChildNodes.Where(node => node.NodeType == HtmlNodeType.Element && node.Name == "a" && node.Attributes.Contains("href")).FirstOrDefault();
                if (animeLinkTag == null)
                {
                    throw new Exception(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
                }

                string animeName = Utils.DecodeHtmlBody(animeNameTag.InnerText);
                string possiblyRelativeUrl = Utils.DecodeHtmlAttribute(animeLinkTag.Attributes["href"].Value);
                return new AnimeStreamInfo(animeName, possiblyRelativeUrl, StreamingService.AnimeNetwork);
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
