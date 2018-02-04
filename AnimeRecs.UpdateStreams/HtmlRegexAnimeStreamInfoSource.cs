using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class HtmlRegexAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        private string Url { get; set; }
        private Regex AnimeRegex { get; set; }
        private string Html { get; set; }
        private StreamingService Service { get; set; }
        private HtmlRegexContext AnimeNameContext { get; set; }
        private HtmlRegexContext UrlContext { get; set; }

        /// <summary>
        /// Regex must have named capture groups called AnimeName and Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="regex"></param>
        public HtmlRegexAnimeStreamInfoSource(string url, Regex animeRegex, string html, StreamingService service, HtmlRegexContext animeNameContext, HtmlRegexContext urlContext)
        {
            Url = url;
            AnimeRegex = animeRegex;
            Html = html;
            Service = service;
            AnimeNameContext = animeNameContext;
            UrlContext = urlContext;
        }

        public Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            // We're not making any http requests in this class and it's not especially CPU intensive, so just run synchronously
            // and wrap the result in a task.
            return Task.FromResult(GetAnimeStreamInfo());
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            Uri animeListUri = new Uri(Url);

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            Match match = AnimeRegex.Match(Html);
            while (match.Success)
            {
                string animeNameRaw = match.Groups["AnimeName"].Value;
                string animeName = DecodeString(animeNameRaw, AnimeNameContext);
                string urlRaw = match.Groups["Url"].Value;
                string url = DecodeString(urlRaw, UrlContext);
                Uri rawUri = new Uri(match.Groups["Url"].Value, UriKind.RelativeOrAbsolute);

                string absoluteUrl;
                if (rawUri.IsAbsoluteUri)
                {
                    absoluteUrl = rawUri.ToString();
                }
                else
                {
                    absoluteUrl = new Uri(animeListUri, rawUri).ToString();
                }

                streams.Add(new AnimeStreamInfo(animeName: animeName, url: absoluteUrl, service: Service));

                match = match.NextMatch();
            }

            if (streams.Count == 0)
            {
                throw new NoMatchingHtmlException("No streams found!");
            }
            return streams;
        }

        private string DecodeString(string str, HtmlRegexContext context)
        {
            switch (context)
            {
                case HtmlRegexContext.Body:
                    return Utils.DecodeHtmlBody(str);
                case HtmlRegexContext.Attribute:
                    return Utils.DecodeHtmlAttribute(str);
                default:
                    return str;
            }
        }
    }
}
