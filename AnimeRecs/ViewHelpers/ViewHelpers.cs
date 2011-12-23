using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;
using AnimeRecs.Models;

namespace AnimeRecs
{
    public static class ViewHelpers
    {
        public static IEnumerable<RecommendedAnimeJson> GetSortedRecommendations(this RecommendorJson recommendor, RecommendationResults results)
        {
            return recommendor.Recommendations.OrderBy(recommendedAnime => results.GetStatus(recommendedAnime.MalId))
                .ThenByDescending(recommendedAnime => recommendedAnime.Rating)
                .ThenByDescending(recommendedAnime => results.GetMyScore(recommendedAnime.MalId))
                .ThenBy(recommendedAnime => recommendedAnime.Name)
                .ToList();
        }
    }
}