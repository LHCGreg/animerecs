using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using Newtonsoft.Json;

namespace AnimeRecs.UpdateStreams
{
    class DaisukiStreamInfoSource : IAnimeStreamInfoSource
    {
        public ICollection<AnimeStreamInfo> GetAnimeStreamInfo()
        {
            string url = "http://www.daisuki.net/bin/wcm/searchAnimeAPI?api=anime_list&searchOptions=&currentPath=%2Fcontent%2Fdaisuki%2Fus%2Fen";

            using (CompressionWebClient webClient = new CompressionWebClient())
            {
                Console.WriteLine("Getting Daisuki anime list.");
                string json = webClient.DownloadString(url);

                DaisukiAnimeQueryJson parsedJson = JsonConvert.DeserializeObject<DaisukiAnimeQueryJson>(json);
                if (parsedJson.response == null)
                {
                    throw new Exception("response list of animes not found in Daisuki JSON.");
                }
                if (parsedJson.response.Count == 0)
                {
                    throw new Exception("No anime found in Daisuki JSON.");
                }

                List<AnimeStreamInfo> streams = new List<AnimeStreamInfo>();
                foreach (DaisukiAnimeJson anime in parsedJson.response)
                {
                    if (anime.animeURL == null)
                    {
                        throw new Exception("animeURL not set in Daisuki JSON.");
                    }
                    if (anime.title == null)
                    {
                        throw new Exception("title not set in Daisuki JSON.");
                    }

                    string animeUrl = string.Format("http://www.daisuki.net{0}", anime.animeURL);
                    streams.Add(new AnimeStreamInfo(anime.title, animeUrl, StreamingService.Daisuki));
                }

                return streams;
            }
        }

        private class DaisukiAnimeQueryJson
        {
            public List<DaisukiAnimeJson> response { get; set; }
        }

        private class DaisukiAnimeJson
        {
            public string animeURL { get; set; }
            public string title { get; set; }
        }
    }
}
