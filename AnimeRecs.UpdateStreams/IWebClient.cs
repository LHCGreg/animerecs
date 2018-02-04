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
    /// Abstraction of HTTP access for easier mocking and switching of HTTP API used.
    /// </summary>
    internal interface IWebClient
    {
        Task<IWebClientResult> GetAsync(WebClientRequest request, CancellationToken cancellationToken);
        Task<IWebClientResult> PostAsync(WebClientRequest request, CancellationToken cancellationToken);
        CookieContainer Cookies { get; }
    }
}
