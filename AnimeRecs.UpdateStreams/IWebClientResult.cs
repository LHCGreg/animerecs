using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    internal interface IWebClientResult : IDisposable
    {
        Task<string> ReadResponseAsStringAsync(CancellationToken cancellationToken);
    }
}
