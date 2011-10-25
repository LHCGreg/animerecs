using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public class MongoRecommendorCache : IRecommendorCache
    {
        public IEnumerable<RecommendorJson> GetRecommendors()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}