using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using AnimeRecs.DAL;
using System.Net;

namespace AnimeRecs.UpdateStreams
{
    abstract class HtmlParsingAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        private string Url { get; set; }
        private string XPath { get; set; }

        protected HtmlParsingAnimeStreamInfoSource(string url, string xpath)
        {
            Url = url;
            XPath = xpath;
        }
        
        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            string responseBody;
            using (CompressionWebClient client = new CompressionWebClient())
            {
                responseBody = client.DownloadString(Url);
            }

            // HTML Agility Pack does not convert "&Auml;" to an A with an umlaut
            // but it doesn't really matter as long as it's readable by the human editing the csv and consistent across program runs.
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNodeCollection matchingNodes = htmlDoc.DocumentNode.SelectNodes(XPath);

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            foreach (HtmlNode matchingNode in matchingNodes)
            {
                AnimeStreamInfo stream = GetStreamInfoFromMatch(matchingNode);
                
                // Convert possibly relative url to an absolute url
                stream = new AnimeStreamInfo(stream.AnimeName, Utils.PossiblyRelativeUrlToAbsoluteUrl(stream.Url, Url), stream.Service);
                
                streams.Add(stream);
            }

            return streams;
        }

        protected abstract AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode);
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
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.