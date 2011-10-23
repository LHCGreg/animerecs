using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class RecommendorCacheFinderFactory : IRecommendationFinderFactory
    {
        private IRecommendorCacheFactory m_cacheFactory;

        public RecommendorCacheFinderFactory(IRecommendorCacheFactory cacheFactory)
        {
            m_cacheFactory = cacheFactory;
        }
        
        public IRecommendationFinder GetRecommendationFinder()
        {
            return new RecommendorCacheRecommendationFinder(m_cacheFactory);
        }
    }
}