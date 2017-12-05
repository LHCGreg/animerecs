using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Not thread-safe. Only supports a limited subset of IWebClient functionality.
    /// </summary>
    internal class ChromeDriverWebClient : IWebClient, IDisposable
    {
        private ChromeDriver _driver;

        public ChromeDriverWebClient(string chromeDriverDirectory)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            _driver = new ChromeDriver(chromeDriverDirectory, chromeOptions);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Not currently supported.
        /// </summary>
        public CookieContainer Cookies => throw new NotImplementedException();

        /// <summary>
        /// Does not currently honor any of the properties of <paramref name="request"/> other than the URL.
        /// Does not currently honor the cancellation token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IWebClientResult> GetAsync(WebClientRequest request, CancellationToken cancellationToken)
        {
            // Selenium's Chrome Driver support has no async support. So do the request on a separate thread.
            return Task.Factory.StartNew(() => GetSync(request), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private IWebClientResult GetSync(WebClientRequest request)
        {
            _driver.Navigate().GoToUrl(request.URL);
            return new StringWebClientResult(_driver.PageSource);
        }

        /// <summary>
        /// Not currently supported.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IWebClientResult> PostAsync(WebClientRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            try
            {
                _driver.Quit();
                _driver.Dispose();
            }
            catch
            {
                ;
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
