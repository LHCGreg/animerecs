using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace AnimeRecs
{
    public class AnimeRecsConfiguration
    {
        public TimeSpan AnimeListCacheExpiration { get; set; }
        public string MongoDatabaseName { get; set; }
        public string MongoRecommendorCacheCollectionName { get; set; }
        public int MinimumRecommendationsNotInCommon { get; set; }
        public int MaximumRecommendorsToReturn { get; set; }
        public decimal DefaultLikedPercent { get; set; }
        public string MalApiUserAgentString { get; set; }

        public string MongoConnectionString { get; set; }

        public AnimeRecsConfiguration()
        {
            ;
        }

        public static AnimeRecsConfiguration FromConfig()
        {
            AnimeRecsConfiguration config = new AnimeRecsConfiguration();
            int malCacheExpirationSeconds = int.Parse(ConfigurationManager.AppSettings["AnimeListCacheExpirationSeconds"], CultureInfo.InvariantCulture);
            int malCacheExpirationMinutes = int.Parse(ConfigurationManager.AppSettings["AnimeListCacheExpirationMinutes"], CultureInfo.InvariantCulture);
            config.AnimeListCacheExpiration = new TimeSpan(hours: 0, minutes: malCacheExpirationMinutes, seconds: malCacheExpirationSeconds);

            config.MongoConnectionString = ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            config.MongoDatabaseName = ConfigurationManager.AppSettings["MongoDbName"];
            config.MongoRecommendorCacheCollectionName = ConfigurationManager.AppSettings["MongoRecommendorCacheCollectionName"];
            config.MinimumRecommendationsNotInCommon = int.Parse(ConfigurationManager.AppSettings["MinimumRecommendationsNotInCommon"], CultureInfo.InvariantCulture);
            config.MaximumRecommendorsToReturn = int.Parse(ConfigurationManager.AppSettings["MaximumRecommendorsToReturn"], CultureInfo.InvariantCulture);
            config.DefaultLikedPercent = decimal.Parse(ConfigurationManager.AppSettings["DefaultGoodPercentile"], CultureInfo.InvariantCulture);
            config.MalApiUserAgentString = ConfigurationManager.AppSettings["MalApiUserAgentString"];

            return config;
        }
    }
}