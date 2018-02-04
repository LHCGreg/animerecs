using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalAnime
    {
        public int MalAnimeId { get; set; }
        public string Title { get; set; }
        public MalApi.MalAnimeType MalAnimeType { get; set; }

        public MalAnime()
        {
            ;
        }

        public MalAnime(int malAnimeId, string title, MalApi.MalAnimeType malAnimeType)
        {
            MalAnimeId = malAnimeId;
            Title = title;
            MalAnimeType = malAnimeType;
        }
    }
}
