using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams.Crunchyroll
{
    /// <summary>
    /// Reads a Crunchyroll html file from disk instead of accessing the Crunchyroll website.
    /// </summary>
    internal class CrunchyrollLocalHtmlStreamInfoSource : IAnimeStreamInfoSource
    {
        private string _crunchyrollLocalHtmlPath;

        public CrunchyrollLocalHtmlStreamInfoSource(string crunchyrollLocalHtmlPath)
        {
            this._crunchyrollLocalHtmlPath = crunchyrollLocalHtmlPath;
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting HTML for {0}", _crunchyrollLocalHtmlPath);

            // Actually opening a file is synchronous.
            // Windows doesn't even provide a way for user-mode code to open a file without blocking.
            // Not sure about other operating systems.
            // ¯\_(ツ)_/¯

            // For that matter, writing to the console could block if stdout is being piped...or maybe even if it's an actual console.
            // Oh well? So much for async all the way down.
            // ¯\_(ツ)_/¯ ¯\_(ツ)_/¯ ¯\_(ツ)_/¯

            // Oh, and StreamReader doesn't support cancellation tokens. (╯°□°）╯︵ ┻━┻

            string html;

            using (FileStream localHtmlStream = new FileStream(_crunchyrollLocalHtmlPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            using (StreamReader localHtmlReader = new StreamReader(localHtmlStream, Encoding.UTF8))
            {
                html = await localHtmlReader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
            }

            var source = new CrunchyrollHtmlStreamInfoSource(html);
            return source.GetAnimeStreamInfo();
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
