using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public interface IRecommendationFinder : IDisposable
    {
        RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList);
    }
}
