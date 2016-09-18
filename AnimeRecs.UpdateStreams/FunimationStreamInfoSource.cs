using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using MiscUtil.Collections;
using HtmlAgilityPack;

namespace AnimeRecs.UpdateStreams
{
    class FunimationStreamInfoSource : IAnimeStreamInfoSource
    {
        public FunimationStreamInfoSource()
        {

        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            HashSet<AnimeStreamInfo> tvStreams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, Tuple<string, string>>(streamInfo => Tuple.Create(streamInfo.Url, streamInfo.AnimeName)));
            HashSet<AnimeStreamInfo> movieStreams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, Tuple<string, string>>(streamInfo => Tuple.Create(streamInfo.Url, streamInfo.AnimeName)));
            string tvUrlTemplate = "http://www.funimation.com/shows/{0}?search=0&order_by=title&order_sort=asc&offset={0}";
            string movieUrlTemplate = "http://www.funimation.com/videos/movies/{0}?search=0&order_by=show_title&order_sort=asc&offset={0}";
            int offset = 0;

            while (true)
            {
                string url = string.Format(tvUrlTemplate, offset);
                HtmlParsingAnimeStreamInfoSource helperSource = new HelperStreamInfoSource(url);

                try
                {
                    ICollection<AnimeStreamInfo> streamsFromThisRequest = helperSource.GetAnimeStreamInfo();
                    tvStreams.UnionWith(streamsFromThisRequest);
                }
                catch (NoMatchingHtmlException)
                {
                    if (tvStreams.Count > 0)
                    {
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }

                offset += 20;
            }

            offset = 0;

            while (true)
            {
                string url = string.Format(movieUrlTemplate, offset);
                HtmlParsingAnimeStreamInfoSource helperSource = new HelperStreamInfoSource(url);

                try
                {
                    ICollection<AnimeStreamInfo> streamsFromThisRequest = helperSource.GetAnimeStreamInfo();
                    movieStreams.UnionWith(streamsFromThisRequest);
                }
                catch (NoMatchingHtmlException)
                {
                    if (movieStreams.Count > 0)
                    {
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }

                offset += 20;
            }

            HashSet<AnimeStreamInfo> allStreams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, Tuple<string, string>>(streamInfo => Tuple.Create(streamInfo.Url, streamInfo.AnimeName)));
            allStreams.UnionWith(tvStreams);
            allStreams.UnionWith(movieStreams);
            return allStreams;
        }

        private class HelperStreamInfoSource : HtmlParsingAnimeStreamInfoSource
        {
            //<div class="popup-heading">
            //    <a href="http://www.funimation.com/shows/king-of-thorn" class="item-title">King of Thorn</a>
            //</div>
            private static string xpath = "//div[contains(@class, 'popup-heading')]/a[contains(@class, 'item-title')]";

            public HelperStreamInfoSource(string url)
                : base(url, xpath)
            {
                Cookies = new CookieCollection() { new Cookie("welcome_page", "1", "/", "www.funimation.com") }; // Needed so Funimation doesn't return the page about Funimation and Crunchyroll teaming up
                Headers = new Dictionary<string, string>(1) { { "X-Requested-With", "XMLHttpRequest" } }; // Needed or else Funimation returns the regular page instead of the ajax results
            }

            protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
            {
                string animeName = HtmlEntity.DeEntitize(matchingNode.InnerText);
                string url = matchingNode.Attributes["href"].Value;
                return new AnimeStreamInfo(animeName, url, StreamingService.Funimation);
            }
        }
    }
}

// Copyright (C) 2016 Greg Najda
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