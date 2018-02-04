using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class CsvRow
    {
        public StreamingService Service { get; private set; }
        public string AnimeName { get; private set; }
        public string Url { get; private set; }
        public MalId MalAnimeId { get; private set; }

        public CsvRow(StreamingService service, string animeName, string url, MalId malAnimeId)
        {
            Service = service;
            AnimeName = animeName;
            Url = url;
            MalAnimeId = malAnimeId;
        }
    }
}
