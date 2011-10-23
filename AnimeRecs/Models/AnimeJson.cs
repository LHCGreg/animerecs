using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class AnimeJson
    {
        public string Name { get; set; }
        public decimal? Rating { get; set; }
        public int? MalId { get; set; }
        public string MalUrl
        {
            get
            {
                if (MalId.HasValue)
                {
                    return string.Format("http://myanimelist.net/anime/{0}", MalId.Value); // TODO: Add url-sanitized anime name at end for a friendlier URL
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public static class MyAnimeListEntryExtensions
    {
        public static AnimeJson ToAnimeJson(this MyAnimeListEntry anime)
        {
            return new AnimeJson()
            {
                Name = anime.Name,
                Rating = anime.Score,
                MalId = anime.Id
            };
        }
    }
}