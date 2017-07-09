using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Gets HTML from a web page and lets the child class process it.
    /// </summary>
    abstract class WebPageStreamInfoSource : IAnimeStreamInfoSource
    {
        private IWebClient WebClient { get; set; }
        protected WebClientRequest Request { get; private set; }

        internal WebPageStreamInfoSource(string url, IWebClient webClient)
        {
            Request = new WebClientRequest(url);
            WebClient = webClient;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            Console.WriteLine("Getting HTML for {0}", Request.URL);
            string responseBody = WebClient.GetString(Request);

            ICollection<AnimeStreamInfo> streams = GetAnimeStreamInfo(responseBody);
            return streams;
        }

        protected abstract ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string html);
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
