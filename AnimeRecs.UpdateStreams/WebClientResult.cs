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
    class WebClientResult : IWebClientResult, IDisposable
    {
        private HttpResponseMessage _response;

        public WebClientResult(HttpResponseMessage response)
        {
            _response = response;
        }

        public Task<string> ReadResponseAsStringAsync(CancellationToken cancellationToken)
        {
            // HttpContent does not support CancellationTokens. WebClient makes sure to not return from http requests
            // until all the content is read into memory so this is ok-ish.
            return _response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _response.Dispose();
        }
    }
}
