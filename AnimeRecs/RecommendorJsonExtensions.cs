using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;

namespace AnimeRecs
{
    public static class RecommendorJsonExtensions
    {
        public static string GetMalListUri(this RecommendorJson recommendor)
        {
            return string.Format("http://myanimelist.net/animelist/{0}", Uri.EscapeUriString(recommendor.Name));
        }
    }
}