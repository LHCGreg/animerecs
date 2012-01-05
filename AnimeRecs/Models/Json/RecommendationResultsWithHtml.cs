using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class RecommendationResultsWithHtml
    {
        public string Html { get; set; }

        public decimal? RecommendedCutoff { get; set; }
    }
}