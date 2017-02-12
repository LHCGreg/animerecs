using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Gets HTML from a web page and lets the child class process it.
    /// </summary>
    abstract class WebPageStreamInfoSource : IAnimeStreamInfoSource
    {
        protected string Url { get; private set; }
        private IWebClient WebClient { get; set; }

        public WebPageStreamInfoSource(string url)
            : this(url, new WebClient())
        {

        }

        internal WebPageStreamInfoSource(string url, IWebClient webClient)
        {
            Url = url;
            WebClient = webClient;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            string responseBody;

            Console.WriteLine("Getting HTML for {0}", Url);
            using (IWebClientResult result = WebClient.Get(Url))
            {
                responseBody = result.Content.ReadToEnd();
            }

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
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.