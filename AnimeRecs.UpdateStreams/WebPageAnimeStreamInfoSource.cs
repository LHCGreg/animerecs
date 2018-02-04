using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Gets HTML from a web page and lets the child class process it.
    /// </summary>
    abstract class WebPageStreamInfoSource : IAnimeStreamInfoSource
    {
        private IWebClient WebClient { get; set; }
        protected WebClientRequest Request { get; private set; }

        internal WebPageStreamInfoSource(string url, IWebClient webClient)
        {
            Request = new WebClientRequest(url);
            WebClient = webClient;
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting HTML for {0}", Request.URL);
            string responseBody = await WebClient.GetStringAsync(Request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            ICollection<AnimeStreamInfo> streams = GetAnimeStreamInfo(responseBody);
            return streams;
        }

        protected abstract ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string html);
    }
}
