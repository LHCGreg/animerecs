using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class RecommendationResults
    {
        public decimal? RecommendedCutoff { get; set; }
        public decimal? OkCutoff { get; set; }
        public IList<AnimeJson> Liked { get; set; }
        public IList<AnimeJson> Ok { get; set; }
        public IList<AnimeJson> Disliked { get; set; }
        public IList<RecommendorMatch> BestMatches { get; set; }
    }
}