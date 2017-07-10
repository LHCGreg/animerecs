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
