using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Common
{
    public class RecommendorJson
    {
        public string Name { get; set; }
        public IList<RecommendedAnimeJson> Recommendations { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}