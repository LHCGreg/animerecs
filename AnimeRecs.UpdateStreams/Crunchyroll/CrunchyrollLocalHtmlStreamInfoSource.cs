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
