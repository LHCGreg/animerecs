using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeRecs.Models;
using System.ComponentModel.DataAnnotations;
using AnimeCompatibility;
using MongoDB.Driver;
using System.Configuration;
using AnimeRecs.Common;

namespace AnimeRecs.Controllers
{
    // TODO:
    // Allow input of good/ok cutoff or liked/disliked percentiles
    
    public class FindBestMatchController : Controller
    {
        private IRecommendationFinderFactory m_finderFactory;

        public FindBestMatchController()
        {
            string mongoConnectionString = ConfigurationManager.ConnectionStrings["Mongo"].ToString();
            MongoServer mongoServer = MongoServer.Create(mongoConnectionString);

            string dbName = "AnimeRecs";
            MongoDatabase recommendorDb = mongoServer.GetDatabase(dbName);

            string collectionName = "Recommendors";
            MongoCollection<RecommendorJson> recommendorCollection = recommendorDb.GetCollection<RecommendorJson>(collectionName);

            m_finderFactory = new RecommendorCacheFinderFactory(new MongoRecommendorCache(recommendorCollection), disposeCache: true);
        }
        
        public FindBestMatchController(IRecommendationFinderFactory finderFactory)
        {
            m_finderFactory = finderFactory;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_finderFactory.Dispose();
            }

            base.Dispose(disposing);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Index([Required] MalApiJson animeList)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(new { error = true });
            }

            List<MyAnimeListEntry> malList = animeList.anime.Select(malApiAnimeJson =>
                new MyAnimeListEntry()
                {
                    Id = malApiAnimeJson.id,
                    Name = malApiAnimeJson.title,
                    Score = malApiAnimeJson.score != 0 ? malApiAnimeJson.score : (decimal?)null,
                    Status = ParseCompletionStatus(malApiAnimeJson.watched_status)
                })
                .ToList();


            using (IRecommendationFinder recFinder = m_finderFactory.GetRecommendationFinder())
            {
                RecommendationResults results = recFinder.GetRecommendations(malList);
                return Json(results);
            }
        }

        internal static CompletionStatus ParseCompletionStatus(string status)
        {
            if (status.Equals("completed", StringComparison.OrdinalIgnoreCase))
            {
                return CompletionStatus.Completed;
            }
            else if (status.Equals("watching", StringComparison.OrdinalIgnoreCase))
            {
                return CompletionStatus.Watching;
            }
            else if (status.Equals("dropped", StringComparison.OrdinalIgnoreCase))
            {
                return CompletionStatus.Dropped;
            }
            else if (status.Equals("plan to watch", StringComparison.OrdinalIgnoreCase))
            {
                return CompletionStatus.PlanToWatch;
            }
            else if (status.Equals("on-hold", StringComparison.OrdinalIgnoreCase))
            {
                return CompletionStatus.OnHold;
            }
            else
            {
                return CompletionStatus.Unknown;
            }
        }
    }
}
