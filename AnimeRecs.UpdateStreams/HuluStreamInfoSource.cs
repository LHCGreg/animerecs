using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using AnimeRecs.DAL;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AnimeRecs.UpdateStreams
{
    class HuluStreamInfoSource : IAnimeStreamInfoSource
    {
        private string _oauthToken;
        
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

        public HuluStreamInfoSource()
        {
            ;
        }

        private void EnsureOauthToken()
        {
            if(_oauthToken != null)
                return;

            using (CompressionWebClient webClient = new CompressionWebClient())
            {
                string huluPageHtml = webClient.DownloadString("http://www.hulu.com/tv/genres/anime");
                Regex oathTokenRegex = new Regex("w.API_DONUT = '(?<Token>[^']*)'");
                Match m = oathTokenRegex.Match(huluPageHtml);
                if (!m.Success)
                {
                    throw new Exception("w.API_DONUT not found in Hulu HTML. The page probably changed and the code for getting Hulu anime needs to be updaed.");
                }

                _oauthToken = m.Groups["Token"].ToString();
            }
        }

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            ICollection<AnimeStreamInfo> animeStreams = GetAnimeStreamInfo("shows", "Anime");
            HashSet<string> urls = new HashSet<string>(animeStreams.Select(stream => stream.Url));

            streams.AddRange(animeStreams);
            // Ugh, Hulu puts some anime in "Animation and Cartoons" instead of "Anime".
            // And some in both!

            ICollection<AnimeStreamInfo> animationAndCartoonStreams = GetAnimeStreamInfo("shows", "Animation and Cartoons");
            streams.AddRange(animationAndCartoonStreams.Where(stream => !urls.Contains(stream.Url)));

            ICollection<AnimeStreamInfo> animeMovieStreams = GetAnimeStreamInfo("movies", "Anime");
            streams.AddRange(animeMovieStreams.Where(stream => !urls.Contains(stream.Url)));

            ICollection<AnimeStreamInfo> animationAndCartoonMovieStreams = GetAnimeStreamInfo("movies", "Animation and Cartoons");
            streams.AddRange(animationAndCartoonMovieStreams.Where(stream => !urls.Contains(stream.Url)));
            return streams;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">movies or shows</param>
        /// <param name="genre"></param>
        /// <returns></returns>
        private ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string type, string genre)
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

            EnsureOauthToken();

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();

            using (CompressionWebClient webClient = new CompressionWebClient())
            {
                int position = 0;
                int numAnimes = int.MaxValue;

                while (position < numAnimes)
                {
                    string url = string.Format(
                        CultureInfo.InvariantCulture,
                        "http://www.hulu.com/mozart/v1.h2o/{0}?exclude_hulu_content=1&genre={1}&sort=release_with_popularity&_language=en&_region=us&items_per_page=100&position={2}&region=us&locale=en&language=en&access_token={3}",
                        type, Uri.EscapeDataString(genre), position, Uri.EscapeDataString(_oauthToken));

                    string jsonString = webClient.DownloadString(url);
                    HuluAnimeResultsJsonRoot json = JsonConvert.DeserializeObject<HuluAnimeResultsJsonRoot>(jsonString);

                    if (json.data.Count == 0)
                    {
                        throw new Exception(string.Format("Did not get any hulu anime from url {0}", url));
                    }

                    numAnimes = json.total_count;
                    
                    foreach (var data in json.data)
                    {
                        string animeName = data.show.name;
                        string animeUrl = string.Format("http://www.hulu.com/{0}", data.show.canonical_name);
                        streams.Add(new AnimeStreamInfo(animeName: animeName, url: animeUrl, service: StreamingService.Hulu));
                        position++;
                    }
                }
            }

            return streams;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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