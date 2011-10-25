using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public class RecommendationResults
    {
        public decimal? RecommendedCutoff { get; set; }
        public decimal? OkCutoff { get; set; }
        public IList<RecommendedAnimeJson> Liked { get; set; }
        public IList<RecommendedAnimeJson> Ok { get; set; }
        public IList<RecommendedAnimeJson> Disliked { get; set; }
        public IList<RecommendorMatch> BestMatches { get; set; }
    }
}