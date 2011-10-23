using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class Recommendor
    {
        public string Name { get; set; }
        public IList<AnimeJson> Recommendations { get; set; }
    }
}