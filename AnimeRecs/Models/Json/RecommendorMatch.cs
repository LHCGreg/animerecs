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
        public decimal PercentLiked { get; set; }
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1}%)", Recommendor.Name, PercentLiked);
        }
    }
}