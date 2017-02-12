using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class HtmlRegexAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        private string Url { get; set; }
        private Regex AnimeRegex { get; set; }
        private string Html { get; set; }
        private StreamingService Service { get; set; }
        private HtmlRegexContext AnimeNameContext { get; set; }
        private HtmlRegexContext UrlContext { get; set; }

        /// <summary>
        /// Regex must have named capture groups called AnimeName and Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="regex"></param>
        public HtmlRegexAnimeStreamInfoSource(string url, Regex animeRegex, string html, StreamingService service, HtmlRegexContext animeNameContext, HtmlRegexContext urlContext)
        {
            Url = url;
            AnimeRegex = animeRegex;
            Html = html;
            Service = service;
            AnimeNameContext = animeNameContext;
            UrlContext = urlContext;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            Uri animeListUri = new Uri(Url);

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            Match match = AnimeRegex.Match(Html);
            while (match.Success)
            {
                string animeNameRaw = match.Groups["AnimeName"].Value;
                string animeName = DecodeString(animeNameRaw, AnimeNameContext);
                string urlRaw = match.Groups["Url"].Value;
                string url = DecodeString(urlRaw, UrlContext);
                Uri rawUri = new Uri(match.Groups["Url"].Value, UriKind.RelativeOrAbsolute);

                string absoluteUrl;
                if (rawUri.IsAbsoluteUri)
                {
                    absoluteUrl = rawUri.ToString();
                }
                else
                {
                    absoluteUrl = new Uri(animeListUri, rawUri).ToString();
                }

                streams.Add(new AnimeStreamInfo(animeName: animeName, url: absoluteUrl, service: Service));

                match = match.NextMatch();
            }

            if (streams.Count == 0)
            {
                throw new NoMatchingHtmlException("No streams found!");
            }
            return streams;
        }

        private string DecodeString(string str, HtmlRegexContext context)
        {
            switch (context)
            {
                case HtmlRegexContext.Body:
                    return WebUtility.HtmlDecode(str);
                case HtmlRegexContext.Attribute:
                    return str.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
                default:
                    return str;
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
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.