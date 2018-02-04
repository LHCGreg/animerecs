using System;
using System.Collections.Generic;
using System.Linq;
using MalApi;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalListEntry
    {
        public int MalAnimeId { get; set; }
        public byte? Rating { get; set; }
        public CompletionStatus Status { get; set; }
        public short NumEpisodesWatched { get; set; }

        public MalListEntry()
        {
            ;
        }

        public MalListEntry(int malAnimeId, byte? rating, CompletionStatus status, short numEpisodesWatched)
        {
            MalAnimeId = malAnimeId;
            Rating = rating;
            Status = status;
            NumEpisodesWatched = numEpisodesWatched;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", MalAnimeId, Rating.HasValue ? Rating.Value.ToString() : "?", Status);
        }
    }
}
