using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace AnimeRecs.Common
{
    public class RecommendedAnimeJson
    {
        public string Name { get; set; }
        public decimal? Rating { get; set; }
        public int MalId { get; set; }
        public string MalUrl
        {
            get
            {
                // TODO: Add url-sanitized anime name at end for a friendlier URL
                return string.Format("http://myanimelist.net/anime/{0}", MalId.ToString(CultureInfo.InvariantCulture));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}