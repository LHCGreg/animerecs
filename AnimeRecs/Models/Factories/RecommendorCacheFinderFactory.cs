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

        //private int m_minimumRecsSeen = RecommendorCacheRecommendationFinder.DefaultMinimumRecsSeen;
        //public int MinimumRecsSeen { get { return m_minimumRecsSeen; } set { m_minimumRecsSeen = value; } }

        private int m_minimumRecsNotSeen = RecommendorCacheRecommendationFinder.DefaultMinimumRecsNotSeen;
        public int MinimumRecsNotSeen { get { return m_minimumRecsNotSeen; } set { m_minimumRecsNotSeen = value; } }

        private int m_maximumRecommendorsToReturn = RecommendorCacheRecommendationFinder.DefaultMaximumRecommendorsToReturn;
        public int MaximumRecommendorsToReturn { get { return m_maximumRecommendorsToReturn; } set { m_maximumRecommendorsToReturn = value; } }

        public RecommendorCacheFinderFactory(IRecommendorCache recommendorCache, bool disposeCache = true)
        {
            m_cache = recommendorCache;
            m_disposeCache = disposeCache;
        }
        
        public IRecommendationFinder GetRecommendationFinder()
        {
            return new RecommendorCacheRecommendationFinder(m_cache, false)
            {
                //MinimumRecsSeen = this.MinimumRecsSeen,
                MinimumRecsNotSeen = this.MinimumRecsNotSeen,
                MaximumRecommendorsToReturn = this.MaximumRecommendorsToReturn
            };
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