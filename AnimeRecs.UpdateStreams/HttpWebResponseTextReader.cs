using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// A TextReader that returns text from an HTTP response body.
    /// </summary>
    class HttpWebResponseTextReader : TextReader
    {
        private HttpWebResponse _response;
        private Stream _responseStream;
        private TextReader _responseReader;

        public HttpWebResponseTextReader(HttpWebResponse response)
        {
            _response = response;
            try
            {
                _responseStream = response.GetResponseStream();
                _responseReader = new StreamReader(_responseStream, Encoding.UTF8); // XXX: Shouldn't be hardcoding UTF-8
            }
            catch (Exception)
            {
                if (response != null) response.Dispose();
                if (_responseStream != null) _responseStream.Dispose();
                if (_responseReader != null) _responseReader.Dispose();
                throw;
            }
        }

        public override void Close()
        {
            _responseReader.Dispose();
            _responseStream.Dispose();
            _response.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _responseReader.Dispose();
                _responseStream.Dispose();
                _response.Dispose();
            }
        }

        public override int Peek()
        {
            return _responseReader.Peek();
        }

        public override int Read()
        {
            return _responseReader.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return _responseReader.Read(buffer, index, count);
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return _responseReader.ReadAsync(buffer, index, count);
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return _responseReader.ReadBlock(buffer, index, count);
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            return _responseReader.ReadBlockAsync(buffer, index, count);
        }

        public override string ReadLine()
        {
            return _responseReader.ReadLine();
        }

        public override Task<string> ReadLineAsync()
        {
            return _responseReader.ReadLineAsync();
        }

        public override string ReadToEnd()
        {
            return _responseReader.ReadToEnd();
        }

        public override Task<string> ReadToEndAsync()
        {
            return _responseReader.ReadToEndAsync();
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
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.
