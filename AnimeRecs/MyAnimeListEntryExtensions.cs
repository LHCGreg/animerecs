using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;
using AnimeCompatibility;

namespace AnimeRecs
{
    public static class MyAnimeListEntryExtensions
    {
        public static RecommendedAnimeJson ToAnimeJson(this MyAnimeListEntry anime)
        {
            return new RecommendedAnimeJson()
            {
                Name = anime.Name,
                Rating = anime.Score,
                MalId = anime.Id
            };
        }
    }
}