using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text.RegularExpressions;

namespace AnimeRecs.MalApi
{
    public class MyAnimeListApi : IMyAnimeListApi
    {
        private const string m_malAppInfoUri = "http://myanimelist.net/malappinfo.php?status=all&type=anime";
        private const string m_recentOnlineUsersUri = "http://myanimelist.net/users.php";

        private static Regex RecentOnlineUsersRegex = new Regex("myanimelist.net/profile/(?<Username>[^\"]+)\">\\k<Username>",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>
        /// What to set the user agent http header to in API requests. Null to use the default .NET user agent.
        /// </summary>
        public string UserAgent { get; set; }

        private int m_timeoutInMs = 15 * 1000;
        public int TimeoutInMs { get { return m_timeoutInMs; } set { m_timeoutInMs = value; } }

        public MyAnimeListApi()
        {
            ;
        }

        private HttpWebRequest InitNewRequest(string uri, string method)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            if (UserAgent != null)
            {
                request.UserAgent = UserAgent;
            }
            request.Timeout = TimeoutInMs;
            request.ReadWriteTimeout = TimeoutInMs;
            request.Method = method;
            request.KeepAlive = false;

            // Very important optimization! Time to get an anime list of ~150 entries 2.6s -> 0.7s
            request.AutomaticDecompression = DecompressionMethods.GZip;

            return request;
        }

        private TReturn ProcessRequest<TReturn>(HttpWebRequest request, Func<string, TReturn> processingFunc, string baseErrorMessage)
        {
            string responseBody = null;
            try
            {
                Logging.Log.DebugFormat("Starting MAL request to {0}", request.RequestUri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Logging.Log.DebugFormat("Got response. Status code = {0}.", response.StatusCode);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new MalApiException(string.Format("{0} Status code was {1}.", baseErrorMessage, response.StatusCode));
                }

                using (Stream responseBodyStream = response.GetResponseStream())
                using (StreamReader responseBodyReader = new StreamReader(responseBodyStream, Encoding.UTF8))
                {
                    // XXX: Shouldn't be hardcoding UTF-8
                    responseBody = responseBodyReader.ReadToEnd();
                }

                Logging.Log.Debug("Read response body.");

                return processingFunc(responseBody);
            }
            catch (MalUserNotFoundException)
            {
                throw;
            }
            catch (MalApiException)
            {
                // Log the body of the response returned by the API server if there was an error.
                // Don't log it otherwise, logs could get big then.
                if (responseBody != null)
                {
                    Logging.Log.DebugFormat("Response body:{0}{1}", Environment.NewLine, responseBody);
                }
                throw;
            }
            catch (Exception ex)
            {
                if (responseBody != null)
                {
                    Logging.Log.DebugFormat("Response body:{0}{1}", Environment.NewLine, responseBody);
                }
                throw new MalApiException(string.Format("{0} {1}", baseErrorMessage, ex.Message), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="AnimeCompatibility.MalUserNotFoundException"></exception>
        /// <exception cref="AnimeCompatibility.MalApiException"></exception>
        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            string userInfoUri = m_malAppInfoUri + "&u=" + Uri.EscapeDataString(user);

            Logging.Log.InfoFormat("Getting anime list for MAL user {0} using URI {1}", user, userInfoUri);

            HttpWebRequest request = InitNewRequest(userInfoUri, "GET");

            Func<string, MalUserLookupResults> responseProcessingFunc = (xml) =>
            {
                using (TextReader xmlTextReader = new StringReader(xml))
                    return ParseAnimeListXml(xmlTextReader, user);
            };
            MalUserLookupResults parsedList = ProcessRequest(request, responseProcessingFunc,
                baseErrorMessage: string.Format("Failed getting anime list for user {0} using url {1}", user, userInfoUri));

            Logging.Log.InfoFormat("Successfully retrieved anime list for user {0}", user);
            return parsedList;
        }

        internal MalUserLookupResults ParseAnimeListXml(TextReader xml, string user)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(myanimelist));
            Logging.Log.Trace("Created XML deserializer.");

            myanimelist animeList;
            using (XmlReader reader = XmlReader.Create(xml))
            {
                Logging.Log.Trace("Created XML reader.");
                try
                {
                    animeList = (myanimelist)serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    throw new MalApiException(string.Format("Error deserializing anime list XML: {0}", ex.Message), ex);
                }
            }

            Logging.Log.Trace("XML parsed.");

            if (animeList.Error != null)
            {
                if (animeList.Error.Equals("Invalid username", StringComparison.OrdinalIgnoreCase))
                {
                    throw new MalUserNotFoundException(string.Format("No MAL list exists for {0}.", user));
                }
                else
                {
                    throw new MalApiException(animeList.Error);
                }
            }

            // Need to grab other stuff from XML here

            myanimelistMyinfo userInfo = (myanimelistMyinfo)animeList.Items.Where(item => item is myanimelistMyinfo).FirstOrDefault();
            if (userInfo == null)
            {
                throw new MalApiException("Error deserializing anime list XML: <myinfo> tag not present.");
            }

            MalUserLookupResults results = new MalUserLookupResults();

            if (userInfo.UserId == null)
            {
                throw new MalApiException("Error deserializing anime list XML: user id was not present.");
            }
            results.UserId = userInfo.UserId.Value;

            if (userInfo.Username == null)
            {
                throw new MalApiException("Error deserializing anime list XML: user name was not present.");
            }
            results.CanonicalUserName = userInfo.Username;

            HashSet<MyAnimeListEntry> parsedList = new HashSet<MyAnimeListEntry>(from anime in animeList.Items
                                                                                 where anime is myanimelistAnime
                                                                                 select XmlEntryToRegularEntry((myanimelistAnime)anime));

            results.AnimeList = parsedList;

            return results;
        }

        private MyAnimeListEntry XmlEntryToRegularEntry(myanimelistAnime xmlEntry)
        {
            MyAnimeListEntry regularEntry = new MyAnimeListEntry();

            if (xmlEntry.SeriesAnimeDbId == null)
                throw new MalApiException("Error deserializing anime list XML: series id was not present.");
            regularEntry.AnimeInfo.AnimeId = xmlEntry.SeriesAnimeDbId.Value;

            if (xmlEntry.series_title == null)
                throw new MalApiException("Error deserializing anime list XML: series title was not present.");
            regularEntry.AnimeInfo.Title = xmlEntry.series_title;

            if (xmlEntry.SeriesType == null)
                throw new MalApiException("Error deserializing anime list XML: series type was not present.");
            regularEntry.AnimeInfo.Type = (MalAnimeType)xmlEntry.SeriesType.Value;

            regularEntry.Score = xmlEntry.MyScore;

            if (xmlEntry.Status == null)
                throw new MalApiException("Error deserializing anime list XML: status not present.");
            regularEntry.Status = (CompletionStatus)xmlEntry.Status.Value;

            if (xmlEntry.NumEpisodesWatched == null)
                throw new MalApiException("Error deserializing anime list XML: number of episodes watched not present.");
            regularEntry.NumEpisodesWatched = xmlEntry.NumEpisodesWatched.Value;

            return regularEntry;
        }

        public RecentUsersResults GetRecentOnlineUsers()
        {
            Logging.Log.InfoFormat("Getting list of recent online MAL users using URI {0}", m_recentOnlineUsersUri);

            HttpWebRequest request = InitNewRequest(m_recentOnlineUsersUri, "GET");

            RecentUsersResults recentUsers = ProcessRequest(request, ScrapeUsersFromHtml,
                baseErrorMessage: string.Format("Failed getting list of recent MAL users."));

            Logging.Log.Info("Successfully got list of recent online MAL users.");
            return recentUsers;
        }

        private RecentUsersResults ScrapeUsersFromHtml(string recentUsersHtml)
        {
            List<string> users = new List<string>();
            MatchCollection userMatches = RecentOnlineUsersRegex.Matches(recentUsersHtml);
            foreach (Match userMatch in userMatches)
            {
                string username = userMatch.Groups["Username"].ToString();
                users.Add(username);
            }

            if (users.Count == 0)
            {
                throw new MalApiException("0 users found in recent users page html.");
            }

            return new RecentUsersResults()
            {
                RecentUsers = users
            };
        }

        public void Dispose()
        {
            ;
        }
    }
}

/*
 Copyright 2011 Greg Najda

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/