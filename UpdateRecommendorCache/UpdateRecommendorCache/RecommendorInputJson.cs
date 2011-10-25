using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.UpdateCache
{
    public class RecommendorInputJson
    {
        public string MalName { get; set; }
        public double? RecommendedPercentile { get; set; }
        public decimal? RecommendedCutoff { get; set; }
    }
}
