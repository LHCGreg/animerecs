using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AnimeRecs.UpdateStreams.Crunchyroll
{
    class CrunchyrollStreamInfoSource : IAnimeStreamInfoSource
    {
        private const string LoginUrl = "https://www.crunchyroll.com/?a=formhandler";

        private IWebClient _webClient;
        
        public CrunchyrollStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            // Need to log in to Crunchyroll because anime classified as "mature content" is not listed
            // to unregistered users.
            
            // Read username and password
            
            string username = GetUsername();
            string password = GetPassword();

            // POST to login, get cookies
            Login(username, password);

            // Use cookies in GET
            var source = new CrunchyrollLoggedInStreamInfoSource(loggedInWebClient: _webClient);
            return source.GetAnimeStreamInfo();
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

        private void Login(string username, string password)
        {
            Console.WriteLine("Logging into Crunchyroll.");
            WebClientRequest request = new WebClientRequest(LoginUrl);
            request.UserAgent = "animerecs.com stream update tool";
            request.PostParameters.Add(new KeyValuePair<string, string>("formname", "RpcApiUser_Login"));
            request.PostParameters.Add(new KeyValuePair<string, string>("fail_url", "http://www.crunchyroll.com/login"));
            request.PostParameters.Add(new KeyValuePair<string, string>("name", username));
            request.PostParameters.Add(new KeyValuePair<string, string>("password", password));

            using (var response = _webClient.Post(request))
            {
                // don't care about response text, just the cookie
            }

            CookieCollection cookies = _webClient.Cookies.GetCookies(new Uri("http://www.crunchyroll.com"));
            if (cookies["sess_id"] == null)
            {
                throw new Exception("Crunchyroll sess_id cookie was not set after logging in, maybe username/password was wrong.");
            }
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
