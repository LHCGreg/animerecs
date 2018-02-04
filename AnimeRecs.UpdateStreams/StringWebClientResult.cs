using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    internal class StringWebClientResult : IWebClientResult
    {
        private string _responseString;

        public StringWebClientResult(string responseString)
        {
            _responseString = responseString;
        }

        public Task<string> ReadResponseAsStringAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseString);
        }

        public void Dispose()
        {
            ;
        }
    }
}
