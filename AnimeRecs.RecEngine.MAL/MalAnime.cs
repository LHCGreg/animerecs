using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalAnime
    {
        public int MalAnimeId { get; private set; }
        public MalAnimeType Type { get; private set; }
        public string Title { get; private set; }

        public MalAnime(int malAnimeId, MalAnimeType type, string title)
        {
            MalAnimeId = malAnimeId;
            Type = type;
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
