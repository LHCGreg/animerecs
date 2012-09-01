using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    abstract class HttpRegexAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        protected string Url { get; private set; }
        protected Regex AnimeRegex { get; private set; }
        protected StreamingService Service { get; private set; }
        protected HttpRegexContext AnimeNameContext { get; private set; }
        protected HttpRegexContext UrlContext { get; private set; }

        /// <summary>
        /// Regex must have a named capture group called AnimeName and Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="regex"></param>
        protected HttpRegexAnimeStreamInfoSource(string url, Regex animeRegex, StreamingService service, HttpRegexContext animeNameContext, HttpRegexContext urlContext)
        {
            Url = url;
            AnimeRegex = animeRegex;
            Service = service;
            AnimeNameContext = animeNameContext;
            UrlContext = urlContext;
        }
        
        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);

            request.Method = "GET";
            request.KeepAlive = false;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string responseBody = null;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("HTTP status code {0}", response.StatusCode));
                }

                using (Stream responseBodyStream = response.GetResponseStream())
                using (StreamReader responseBodyReader = new StreamReader(responseBodyStream, Encoding.UTF8))
                {
                    // XXX: Shouldn't be hardcoding UTF-8
                    responseBody = responseBodyReader.ReadToEnd();
                }
            }

            ICollection<AnimeStreamInfo> streams = GetAnimeStreamInfo(responseBody);
            return streams;
        }

        // Internal for unit testing
        internal ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string responseBody)
        {
            Uri animeListUri = new Uri(Url);
            
            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            Match match = AnimeRegex.Match(responseBody);
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
                throw new Exception("No streams found!");
            }
            return streams;
        }

        private string DecodeString(string str, HttpRegexContext context)
        {
            switch (context)
            {
                case HttpRegexContext.Body:
                    return WebUtility.HtmlDecode(str);
                case HttpRegexContext.Attribute:
                    return str.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
                default:
                    return str;
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
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