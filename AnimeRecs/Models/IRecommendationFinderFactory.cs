using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.Models
{
    public interface IRecommendationFinderFactory
    {
        IRecommendationFinder GetRecommendationFinder();
    }
}
