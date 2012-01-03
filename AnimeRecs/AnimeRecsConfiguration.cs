using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimeRecs
{
    public class AnimeRecsConfiguration
    {
        public TimeSpan AnimeListCacheExpiration { get; private set; }
        public string MongoDatabaseName { get; private set; }
        public string MongoRecommendorCacheCollectionName { get; private set; }
        public int MinimumRecommendationsNotInCommon { get; private set; }
        public int MaximumRecommendorsToReturn { get; private set; }
        public decimal DefaultLikedPercent { get; private set; }
        public string MalApiUserAgentString { get; private set; }

        public string MongoConnectionString { get; private set; }
    }
}