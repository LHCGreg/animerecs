using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using AnimeRecs.Models;
using AnimeRecs.Common;
using AnimeCompatibility;
using MongoDB.Driver;

namespace AnimeRecs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool m_errorOnStarting = false;
        public static bool ErrorOnStarting { get { return m_errorOnStarting; } set { m_errorOnStarting = value; } }
        
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();

                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);

                ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
                ModelBinders.Binders.Add(typeof(decimal?), new NullableDecimalModelBinder());

                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<RecommendorJson>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Name));
                });

                OfficialMalApi api = null;
                CachingMyAnimeListApi cachingApi = null;
                NoConcurrentFetchesOfSameUserMalApi dosProtectionApi = null;
                RecommendorCacheFinderFactory recommendationFinderFactory = null;

                try
                {
                    AppGlobals.Config = AnimeRecsConfiguration.FromConfig();
                    
                    api = new OfficialMalApi() { UserAgent = AppGlobals.Config.MalApiUserAgentString, TimeoutInMs = 5000 };
                    cachingApi = new CachingMyAnimeListApi(api, expiration: AppGlobals.Config.AnimeListCacheExpiration, ownApi: true);
                    dosProtectionApi = new NoConcurrentFetchesOfSameUserMalApi(cachingApi);

                    AppGlobals.MalApiFactory = new SingleMyAnimeListApiFactory(dosProtectionApi);

                    MongoServer mongoServer = MongoServer.Create(AppGlobals.Config.MongoConnectionString);
                    MongoDatabase recommendorDb = mongoServer.GetDatabase(AppGlobals.Config.MongoDatabaseName);
                    MongoCollection<RecommendorJson> recommendorCollection = recommendorDb.GetCollection<RecommendorJson>(AppGlobals.Config.MongoRecommendorCacheCollectionName);

                    recommendationFinderFactory = new RecommendorCacheFinderFactory(
                        new MongoRecommendorCache(recommendorCollection), disposeCache: true);
                    recommendationFinderFactory.MinimumRecsNotSeen = AppGlobals.Config.MinimumRecommendationsNotInCommon;
                    recommendationFinderFactory.MaximumRecommendorsToReturn = AppGlobals.Config.MaximumRecommendorsToReturn;

                    AppGlobals.RecommendationFinderFactory = recommendationFinderFactory;
                }
                catch (Exception)
                {
                    if (dosProtectionApi != null)
                        dosProtectionApi.Dispose();
                    if (cachingApi != null)
                        cachingApi.Dispose();
                    if (api != null)
                        cachingApi.Dispose();
                    if (recommendationFinderFactory != null)
                        recommendationFinderFactory.Dispose();

                    throw;
                }

            }
            catch (Exception)
            {
                ErrorOnStarting = true;
                throw;
            }
        }

        protected void Application_End()
        {
            if (AppGlobals.MalApiFactory != null)
            {
                AppGlobals.MalApiFactory.Dispose();
            }

            if (AppGlobals.RecommendationFinderFactory != null)
            {
                AppGlobals.RecommendationFinderFactory.Dispose();
            }
        }

        protected void Application_BeginRequest()
        {
            if (ErrorOnStarting)
            {
                Response.StatusCode = 503;
                Response.Write("Oops! Something went terribly wrong. A site admin will fix this problem shortly.");
                Response.End();
            }
        }
    }
}