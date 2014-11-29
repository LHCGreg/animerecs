using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AnimeRecs.DAL;
using RestSharp;

namespace AnimeRecs.UpdateStreams
{
    class CrunchyrollStreamInfoSource : IAnimeStreamInfoSource
    {
        private const string AnimeListUrl = "http://www.crunchyroll.com/videos/anime/alpha?group=all";
        private const string LoginBaseUrl = "https://www.crunchyroll.com";
        private const string LoginResource = "?a=formhandler";
        private const string AnimeRegexString =
            "<a title=\"[^\"]*?\" token=\"shows-portraits\" itemprop=\"url\" href=\"(?<Url>[^\"]*?)\" [^>]*?>\\s*(?<AnimeName>.*?)\\s*?</a>";
        private HtmlRegexAnimeStreamInfoSource _sourceAfterLogin;
        
        public CrunchyrollStreamInfoSource()
        {
            _sourceAfterLogin = new HtmlRegexAnimeStreamInfoSource(AnimeListUrl,
                new Regex(AnimeRegexString, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
                StreamingService.Crunchyroll, animeNameContext: HtmlRegexContext.Body, urlContext: HtmlRegexContext.Attribute);
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            // Need to log in to Crunchyroll because anime classified as "mature content" is not listed
            // to unregistered users.
            
            // Read username and password
            
            string username = GetUsername();
            string password = GetPassword();

            // POST to login, get cookies
            CookieCollection cookies = Login(username, password);

            // Use cookies in GET
            _sourceAfterLogin.Cookies = cookies;
            return _sourceAfterLogin.GetAnimeStreamInfo();
        }

        // For unit testing
        internal ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string responseBody)
        {
            return _sourceAfterLogin.GetAnimeStreamInfo(responseBody);
        }

        private static string GetUsername()
        {
            Console.WriteLine("Crunchyroll username:");
            string username = Console.ReadLine();
            return username;
        }

        private static string GetPassword()
        {
            Console.WriteLine("Crunchyroll password:");
            string password = ReadPassword();
            return password;
        }

        private CookieCollection Login(string username, string password)
        {
            RestClient client = new RestClient(LoginBaseUrl);
            RestRequest request = new RestRequest(LoginResource, Method.POST);
            request.AddParameter("formname", "RpcApiUser_Login", ParameterType.GetOrPost);
            request.AddParameter("fail_url", "http://www.crunchyroll.com/login", ParameterType.GetOrPost);
            request.AddParameter("name", username, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);

            client.CookieContainer = new CookieContainer();
            client.UserAgent = "animerecs.com stream update tool";
            IRestResponse response = client.Execute(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(string.Format("Error logging in to crunchyroll: {0}", response.ErrorMessage));
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(string.Format("Error logging in to crunchroll, HTTP status: {0}", response.StatusDescription));
            }

            CookieCollection cookies = client.CookieContainer.GetCookies(new Uri("http://www.crunchyroll.com"));
            if (cookies["sess_id"] == null)
            {
                throw new Exception("Crunchyroll sess_id cookie was not set after logging in, maybe username/password was wrong.");
            }

            return cookies;
        }

        private static string ReadPassword()
        {
            // If Console.ReadKey returns a ConsoleKeyInfo with KeyChar and Key of 0 on Mono, stdin or stdout is redirected.
            // stdout being redirected affecting the value of Console.ReadKey is a bug (https://bugzilla.xamarin.com/show_bug.cgi?id=12552).
            // Returning 0 when stdin is redirected is also a bug (https://bugzilla.xamarin.com/show_bug.cgi?id=12551).
            // The documented behavior is to throw an InvalidOperationException.

            bool runningOnMono = Type.GetType("Mono.Runtime") != null;
            StringBuilder textEntered = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo key;
                try
                {
                    key = Console.ReadKey(intercept: true);
                }
                catch (InvalidOperationException)
                {
                    // Console.ReadKey throws InvalidOperationException if stdin is not a console
                    // .NET 4.5 provides Console.IsInputRedirected.
                    // Switch to 4.5 once Linux distros start packaging a Mono version that support 4.5.
                    throw new Exception("Cannot prompt for password because stdin is redirected.");
                }
                if (runningOnMono && key.KeyChar == '\0' && (int)key.Key == 0)
                {
                    throw new Exception("Cannot prompt for password because stdin is redirected.");
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (textEntered.Length > 0)
                    {
                        textEntered.Length = textEntered.Length - 1;
                    }
                }
                else
                {
                    char c = key.KeyChar;
                    textEntered.Append(c);
                }
            }

            return textEntered.ToString();
        }
    }
}

// Copyright (C) 2014 Greg Najda
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