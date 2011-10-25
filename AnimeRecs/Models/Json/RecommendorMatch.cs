using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public class RecommendorMatch
    {
        public RecommendorJson Recommendor { get; set; }
        public decimal CompatibilityRating { get; set; }
    }
}