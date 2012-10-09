using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class HuluStreamInfoSource : IAnimeStreamInfoSource
    {
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

        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
            ICollection<AnimeStreamInfo> animeStreams = GetAnimeStreamInfo("Anime");
            HashSet<string> urls = new HashSet<string>(animeStreams.Select(stream => stream.Url));

            streams.AddRange(animeStreams);
            // Ugh, Hulu puts some anime in "Animation and Cartoons" instead of "Anime".
            // And some in both!

            ICollection<AnimeStreamInfo> animationAndCartoonStreams = GetAnimeStreamInfo("Animation+and+Cartoons");
            streams.AddRange(animationAndCartoonStreams.Where(stream => !urls.Contains(stream.Url)));
            return streams;
        }

        private ICollection<AnimeStreamInfo> GetAnimeStreamInfo(string genre)
        {
            // http://www.hulu.com/api/2.0/shows.json?genre=Anime&order=desc&sort=view_count_week&items_per_page=64&position=0
            // count is in total_count
            // process results:
            //   data[x].show.name, data[x].show.canonical_name
            // current position += num results processed
            // if current position < count, repeat

            List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();

            using (CompressionWebClient webClient = new CompressionWebClient())
            {
                int position = 0;
                int numAnimes = int.MaxValue;

                while (position < numAnimes)
                {
                    string url = string.Format(
                        "http://www.hulu.com/api/2.0/shows.json?genre={0}&order=desc&sort=view_count_week&items_per_page=100&position={1}",
                        genre, position);

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