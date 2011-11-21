using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;
using MongoDB.Driver;

namespace AnimeRecs.Models
{
    public class MongoRecommendorCache : IRecommendorCache
    {
        private MongoCollection<RecommendorJson> m_mongoRecommendors;
        
        public MongoRecommendorCache(MongoCollection<RecommendorJson> mongoRecommendors)
        {
            m_mongoRecommendors = mongoRecommendors;
        }
        
        public IEnumerable<RecommendorJson> GetRecommendors()
        {
            return m_mongoRecommendors.FindAllAs<RecommendorJson>();
        }

        public void Dispose()
        {
            ; // No Dispose needed. The Mongo driver manages its own connections
        }
    }
}