using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs.Models
{
    public class MockRecommendorCacheFactory : IRecommendorCacheFactory
    {
        public IRecommendorCache GetCache()
        {
            return new MockRecommendorCache();
        }
    }
}