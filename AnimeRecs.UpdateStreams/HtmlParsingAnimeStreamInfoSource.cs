using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AnimeRecs.UpdateStreams
{
    abstract class HtmlParsingAnimeStreamInfoSource : IAnimeStreamInfoSource
    {
        protected IWebClient WebClient { get; private set; }
        protected string Url { get; private set; }
        protected string XPath { get; private set; }

        protected HtmlParsingAnimeStreamInfoSource(string url, string xpath, IWebClient webClient)
        {
            Url = url;
            XPath = xpath;
            WebClient = webClient;
        }

        /// <summary>
        /// Subclasses can add headers here.
        /// </summary>
        protected virtual void ModifyRequestBeforeSending(WebClientRequest request)
        {

        }
        
        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting HTML for {0}", Url);
            WebClientRequest request = new WebClientRequest(Url);
            ModifyRequestBeforeSending(request);

            string responseBody = await WebClient.GetStringAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNodeCollection matchingNodes = htmlDoc.DocumentNode.SelectNodes(XPath);

            if (matchingNodes == null || matchingNodes.Count == 0)
            {
                throw new NoMatchingHtmlException(string.Format("Could not extract information from {0}. The site's HTML format probably changed.", Url));
            }

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            foreach (HtmlNode matchingNode in matchingNodes)
            {
                AnimeStreamInfo stream = GetStreamInfoFromMatch(matchingNode);
                
                // Convert possibly relative url to an absolute url
                stream = new AnimeStreamInfo(stream.AnimeName, Utils.PossiblyRelativeUrlToAbsoluteUrl(stream.Url, Url), stream.Service);
                
                streams.Add(stream);
            }

            OnStreamsExtracted(htmlDoc, streams);

            return streams;
        }

        protected virtual void OnStreamsExtracted(HtmlDocument htmlDoc, List<AnimeStreamInfo> streams)
        {

        }

        protected abstract AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode);
    }
}
