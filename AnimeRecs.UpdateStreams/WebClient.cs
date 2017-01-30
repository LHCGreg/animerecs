using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    internal class WebClient : IWebClient
    {
        /// <summary>
        /// Set this to send cookies in the web request.
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Set this to add headers to the web request
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        public IWebClientResult Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            request.Method = "GET";
            request.KeepAlive = false;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (Cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(Cookies);
            }

            if (Headers != null)
            {
                foreach (KeyValuePair<string, string> headerAndValue in Headers)
                {
                    request.Headers[headerAndValue.Key] = headerAndValue.Value;
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            TextReader responseReader = null;
            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("HTTP status code {0}", response.StatusCode));
                }

                responseReader = new HttpWebResponseTextReader(response);
                return new WebClientResult(responseReader);
            }
            catch (Exception)
            {
                if (responseReader != null) responseReader.Dispose();
                response.Dispose();
                throw;
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
