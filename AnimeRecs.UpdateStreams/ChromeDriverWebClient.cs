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
