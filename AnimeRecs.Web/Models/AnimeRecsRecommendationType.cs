using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeRecs.Web.Models
{
    public enum AnimeRecsRecommendationType
    {
        // Useful recommendations include "plan to watch", "watching", and "on hold"
        UsefulRecommendation = 0,
        Liked = 1,
        Inconclusive = 2,
        NotLiked = 3
    }
}
