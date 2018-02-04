using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    static class WebClientExtensions
    {
        public static async Task<string> GetStringAsync(this IWebClient webClient, string url, CancellationToken cancellationToken)
        {
            using (IWebClientResult result = await webClient.GetAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            {
                return await result.ReadResponseAsStringAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static async Task<string> GetStringAsync(this IWebClient webClient, WebClientRequest request, CancellationToken cancellationToken)
        {
            using (IWebClientResult result = await webClient.GetAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            {
                return await result.ReadResponseAsStringAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public static Task<IWebClientResult> GetAsync(this IWebClient webClient, string url, CancellationToken cancellationToken)
        {
            return webClient.GetAsync(new WebClientRequest(url), cancellationToken);
        }
    }
}
