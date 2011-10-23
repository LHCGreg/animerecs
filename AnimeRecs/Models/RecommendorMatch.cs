using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class RecommendorMatch
    {
        public Recommendor Recommendor { get; set; }
        public decimal CompatibilityRating { get; set; }
    }
}