using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class RecommendorCacheFinderFactory : IRecommendationFinderFactory
    {
        private IRecommendorCache m_cache;
        private bool m_disposeCache;

        public RecommendorCacheFinderFactory(IRecommendorCache recommendorCache, bool disposeCache = true)
        {
            m_cache = recommendorCache;
            m_disposeCache = disposeCache;
        }
        
        public IRecommendationFinder GetRecommendationFinder()
        {
            return new RecommendorCacheRecommendationFinder(m_cache, false);
        }

        public void Dispose()
        {
            if (m_disposeCache)
            {
                m_cache.Dispose();
            }
        }
    }
}