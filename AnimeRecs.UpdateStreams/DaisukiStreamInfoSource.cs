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

// Copyright (C) 2016 Greg Najda
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
