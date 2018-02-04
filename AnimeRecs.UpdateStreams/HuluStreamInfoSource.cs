using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using AnimeRecs.DAL;
using System.Text.RegularExpressions;
using System.Globalization;
using MiscUtil.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace AnimeRecs.UpdateStreams
{
    class HuluStreamInfoSource : IAnimeStreamInfoSource
    {
        private string _oauthToken;
        private IWebClient _webClient;

        class HuluAnimeResultsJsonRoot
        {
            public int total_count { get; set; }
            public int position { get; set; }

            private IList<HuluAnimeResultsJsonData> m_data = new List<HuluAnimeResultsJsonData>();
            public IList<HuluAnimeResultsJsonData> data { get { return m_data; } set { m_data = value; } }
        }

        class HuluAnimeResultsJsonData
        {
            public HuluAnimeResultsJsonShow show { get; set; }
        }

        class HuluAnimeResultsJsonShow
        {
            public string name { get; set; }
            public string canonical_name { get; set; }
        }

        public HuluStreamInfoSource(IWebClient webClient)
        {
            _webClient = webClient;
        }

        private async Task EnsureOauthTokenAsync(CancellationToken cancellationToken)
        {
            if (_oauthToken != null)
                return;

            Console.WriteLine("Getting Hulu API token.");

            string huluPageHtml = await _webClient.GetStringAsync("http://www.hulu.com/tv/genres/anime", cancellationToken);

            Regex oathTokenRegex = new Regex("w.API_DONUT = '(?<Token>[^']*)'");
            Match m = oathTokenRegex.Match(huluPageHtml);
            if (!m.Success)
            {
                throw new Exception("w.API_DONUT not found in Hulu HTML. The page probably changed and the code for getting Hulu anime needs to be updaed.");
            }

            _oauthToken = m.Groups["Token"].ToString();
        }

        public async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(CancellationToken cancellationToken)
        {
            HashSet<AnimeStreamInfo> streams = new HashSet<AnimeStreamInfo>(new ProjectionEqualityComparer<AnimeStreamInfo, string>(streamInfo => streamInfo.Url, StringComparer.OrdinalIgnoreCase));

            // Potential for parallelizing these 4 operations but it's probably not slowing down the
            // whole program run, it'd be more code, and it's nicer to Hulu to only do one request at a time.

            ICollection<AnimeStreamInfo> animeStreams = await GetAnimeStreamInfoAsync("shows", "Anime", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            streams.UnionWith(animeStreams);

            // Ugh, Hulu puts some anime in "Animation and Cartoons" instead of "Anime".
            // And some in both!

            ICollection<AnimeStreamInfo> animationAndCartoonStreams = await GetAnimeStreamInfoAsync("shows", "Animation and Cartoons", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            streams.UnionWith(animationAndCartoonStreams);

            ICollection<AnimeStreamInfo> animeMovieStreams = await GetAnimeStreamInfoAsync("movies", "Anime", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            streams.UnionWith(animeMovieStreams);

            ICollection<AnimeStreamInfo> animationAndCartoonMovieStreams = await GetAnimeStreamInfoAsync("movies", "Animation and Cartoons", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            streams.UnionWith(animationAndCartoonMovieStreams);

            return streams.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">movies or shows</param>
        /// <param name="genre"></param>
        /// <returns></returns>
        private async Task<ICollection<AnimeStreamInfo>> GetAnimeStreamInfoAsync(string type, string genre, CancellationToken cancellationToken)
        {
            // http://www.hulu.com/mozart/v1.h2o/shows?exclude_hulu_content=1&genre={genre}&sort=release_with_popularity&_language=en&_region=us&items_per_page=100&position=0&region=us&locale=en&language=en&access_token={oathtoken}
            // count is in total_count
            // process results:
            //   data[x].show.name, data[x].show.canonical_name
            // current position += num results processed
            // if current position < count, repeat
            //
            // data[x].show.name is the show's name.
            // data[x].show.canonical_name is how you get the url for the show - http://www.hulu.com/{canonical_name}

            await EnsureOauthTokenAsync(cancellationToken);

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();

            int position = 0;
            int numAnimesInPage;
            do
            {
                string url = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://www.hulu.com/mozart/v1.h2o/{0}?exclude_hulu_content=1&genre={1}&sort=release_with_popularity&_language=en&_region=us&items_per_page=100&position={2}&region=us&locale=en&language=en&access_token={3}",
                    type, Uri.EscapeDataString(genre), position, Uri.EscapeDataString(_oauthToken));

                Console.WriteLine("Getting type genre {0}, type {1}, position {2} from Hulu", genre, type, position);
                string jsonString = await _webClient.GetStringAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                HuluAnimeResultsJsonRoot json = JsonConvert.DeserializeObject<HuluAnimeResultsJsonRoot>(jsonString);

                // You can get no results if the total count on the first page does not agree with the total count on later pages.
                // Additions/removals seem to lag behind on later pages for some reason.
                if (json.data.Count == 0 && json.total_count == 0)
                {
                    throw new Exception(string.Format("Did not get any hulu anime from url {0}", url));
                }
                numAnimesInPage = json.data.Count;

                foreach (var data in json.data)
                {
                    string animeName = data.show.name;
                    string animeUrl = string.Format("http://www.hulu.com/{0}", data.show.canonical_name);
                    streams.Add(new AnimeStreamInfo(animeName: animeName, url: animeUrl, service: StreamingService.Hulu));
                    position++;
                }
            } while (numAnimesInPage > 0);

            return streams;
        }
    }
}
