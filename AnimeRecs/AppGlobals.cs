using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Models;

namespace AnimeRecs
{
    public static class AppGlobals
    {
        public static IMyAnimeListApiFactory MalApiFactory { get; set; }
        public static IRecommendationFinderFactory RecommendationFinderFactory { get; set; }
        public static AnimeRecsConfiguration Config { get; set; }
    }
}