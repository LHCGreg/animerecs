using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public interface IRecommendationFinder
    {
        RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList);
    }
}
