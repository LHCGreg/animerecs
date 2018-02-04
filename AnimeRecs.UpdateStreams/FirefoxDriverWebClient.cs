using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// not thread-safe. Only supports a limited subset of IWebClient functionality.
    /// </summary>
    internal class FirefoxDriverWebClient : IWebClient, IDisposable
    {
        private FirefoxDriver _driver;

        public FirefoxDriverWebClient(string geckoDriverDirectory)
        {
            // Workaround for error in FirefoxDriver when running on .net core:
            // Unhandled Exception: System.TypeInitializationException: The type initializer for 'System.IO.Compression.ZipStorer' threw an exception. --->System.NotSupportedException: No data is available for encoding 437.For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.
            // at System.Text.Encoding.GetEncoding(Int32 codepage)
            // at System.IO.Compression.ZipStorer..cctor()
            // -- - End of inner exception stack trace-- -
            // at System.IO.Compression.ZipStorer.WriteLocalHeader(ZipFileEntry & zipFileEntry)
            // at System.IO.Compression.ZipStorer.AddStream(CompressionMethod compressionMethod, Stream sourceStream, String fileNameInZip, DateTime modificationTimeStamp, String fileEntryComment)
            // at System.IO.Compression.ZipStorer.AddFile(CompressionMethod compressionMethod, String sourceFile, String fileNameInZip, String fileEntryComment)
            // at OpenQA.Selenium.Firefox.FirefoxProfile.ToBase64String()
            // at OpenQA.Selenium.Firefox.FirefoxOptions.GenerateFirefoxOptionsDictionary()
            // at OpenQA.Selenium.Firefox.FirefoxOptions.ToCapabilities()
            // at OpenQA.Selenium.Firefox.FirefoxDriver.ConvertOptionsToCapabilities(FirefoxOptions options)
            // at OpenQA.Selenium.Firefox.FirefoxDriver..ctor(String geckoDriverDirectory, FirefoxOptions options)

            // See https://github.com/SeleniumHQ/selenium/issues/4816

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Environment.SetEnvironmentVariable("MOZ_HEADLESS", "1");
            FirefoxOptions firefoxOptions = new FirefoxOptions()
            {
                Profile = new FirefoxProfile()
                {
                    DeleteAfterUse = true,
                },
            };
            firefoxOptions.AddArgument("--headless");
            firefoxOptions.SetPreference("javascript.enabled", false);

            _driver = new FirefoxDriver(geckoDriverDirectory, firefoxOptions);
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
            // Selenium's Firefox Driver support has no async support. So do the request on a separate thread.
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
