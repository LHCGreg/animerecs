using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using MiscUtil.Collections;
using Newtonsoft.Json;

namespace AnimeRecs.UpdateStreams
{
    class ViewsterStreamInfoSource : IAnimeStreamInfoSource
    {
        private static readonly string InitialPageUrl = "https://www.viewster.com/genre/58/anime/";
        
        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            string apiToken = GetAPIToken();
            return GetAnimeSeries(apiToken);
        }

        private string GetAPIToken()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(InitialPageUrl);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.CookieContainer = new CookieContainer();
            request.Method = "GET";
            request.ReadWriteTimeout = 20 * 1000;
            request.Timeout = 20 * 1000;
            request.UserAgent = "animerecs.com stream update tool";

            Console.WriteLine("Getting Viewster API token.");
            using (WebResponse baseResponse = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)baseResponse;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("HTTP status code {0}", response.StatusCode));
                }

                Cookie apiTokenCookie = response.Cookies["api_token"];
                if (apiTokenCookie == null)
                {
                    throw new Exception("Viewster api_token cookie not set on initial page request. Maybe Viewster changed their site.");
                }

                return WebUtility.UrlDecode(apiTokenCookie.Value);
            }
        }

        private ICollection<AnimeStreamInfo> GetAnimeSeries(string apiToken)
        {
            // https://public-api.viewster.com/series?genreId=58&pageSize=25
            int pageSize = 100;
            int pageIndex = 1;

            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(stream => stream.Url));

            using (CompressionWebClient webClient = new CompressionWebClient())
            {
                webClient.Headers.Add("Auth-token", apiToken);
                webClient.Headers.Add("Accept", "application/json");
                string baseUrl = "https://public-api.viewster.com/series?genreId=58";
                ICollection<AnimeStreamInfo> streamsFromThisRequest;

                do
                {
                    string url = baseUrl + string.Format("&pageSize={0}&pageIndex={1}", pageSize, pageIndex);

                    Console.WriteLine("Getting Viewster anime page {0}.", pageIndex);
                    string json = webClient.DownloadString(url);

                    streamsFromThisRequest = ParseAnimeJson(json);
                    streams.UnionWith(streamsFromThisRequest);

                    pageIndex++;
                } while (streamsFromThisRequest.Count == pageSize);
            }

            if (streams.Count == 0)
            {
                throw new Exception("No Viewster streams found, something must be wrong.");
            }
            return streams;
        }

        private class ViewsterAnimeQueryJson
        {
            public List<ViewsterAnimeJson> Items { get; set; }
        }

        private class ViewsterAnimeJson
        {
            public string OriginId { get; set; }
            public string Title { get; set; }
        }

        private ICollection<AnimeStreamInfo> ParseAnimeJson(string json)
        {
            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            
            ViewsterAnimeQueryJson parsedJson = JsonConvert.DeserializeObject<ViewsterAnimeQueryJson>(json);
            if (parsedJson.Items == null)
            {
                throw new Exception("Items list not present in Viewster JSON.");
            }

            foreach (ViewsterAnimeJson animeJson in parsedJson.Items)
            {
                if (animeJson.Title == null)
                {
                    throw new Exception("Title not present in Viewster JSON.");
                }
                
                if (animeJson.OriginId == null)
                {
                    throw new Exception("OriginId not present in Viewster JSON.");
                }

                string animeUrl = string.Format("https://www.viewster.com/serie/{0}", animeJson.OriginId);
                streams.Add(new AnimeStreamInfo(animeJson.Title, animeUrl, StreamingService.Viewster));
            }

            return streams;
        }
    }
}

// Copyright (C) 2015 Greg Najda
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