using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// thread-safe
    /// </summary>
    internal class WebClient : IWebClient, IDisposable
    {
        private HttpClientHandler _handler;
        private HttpClient _client;

        public WebClient()
        {
            _handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            _handler.CookieContainer = new CookieContainer();
            _client = new HttpClient(_handler);
        }

        public CookieContainer Cookies { get { return _handler.CookieContainer; } }

        private HttpRequestMessage TranslateRequest(WebClientRequest request, HttpMethod method)
        {
            HttpRequestMessage translatedRequest = new HttpRequestMessage(method, request.URL);

            if (request.UserAgent != null)
            {
                if (!translatedRequest.Headers.TryAddWithoutValidation("User-Agent", request.UserAgent))
                {
                    throw new Exception("Failed adding user agent string to request.");
                }
            }

            if (request.Accept != null)
            {
                if (!translatedRequest.Headers.TryAddWithoutValidation("Accept", request.Accept))
                {
                    throw new Exception("Failed adding Accept header to request.");
                }
            }

            if (request.Headers != null)
            {
                foreach (KeyValuePair<string, string> header in request.Headers)
                {
                    if (!translatedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    {
                        throw new Exception(string.Format("Failed adding {0} header with value {1} to request.", header.Key, header.Value));
                    }
                }
            }

            if (method == HttpMethod.Post && request.PostParameters != null && request.PostParameters.Count > 0)
            {
                FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(request.PostParameters);
                translatedRequest.Content = encodedContent;
            }

            return translatedRequest;
        }

        private async Task<IWebClientResult> DoRequestAsync(WebClientRequest request, HttpMethod method, CancellationToken cancellationToken)
        {
            using (HttpRequestMessage translatedRequest = TranslateRequest(request, method))
            {
                HttpResponseMessage response = await _client.SendAsync(translatedRequest, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                try
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(string.Format("Request to {0} failed with status code {1}.", request.URL, response.StatusCode));
                    }
                }
                catch (Exception)
                {
                    response.Dispose();
                    throw;
                }

                return new WebClientResult(response);
            }
        }

        public Task<IWebClientResult> GetAsync(WebClientRequest request, CancellationToken cancellationToken)
        {
            return DoRequestAsync(request, HttpMethod.Get, cancellationToken);
        }

        public Task<IWebClientResult> PostAsync(WebClientRequest request, CancellationToken cancellationToken)
        {
            return DoRequestAsync(request, HttpMethod.Post, cancellationToken);
        }

        public void Dispose()
        {
            _client.Dispose();
            _handler.Dispose();
        }
    }
}
