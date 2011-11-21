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
    public class FindBestMatchController : Controller
    {
        private IRecommendationFinderFactory m_finderFactory;
        private IMyAnimeListApiFactory m_malApiFactory;

        public FindBestMatchController()
        {
            m_finderFactory = ApplicationGlobals.RecommendationFinderFactory;
            m_malApiFactory = ApplicationGlobals.MalApiFactory;
        }
        
        public FindBestMatchController(IRecommendationFinderFactory finderFactory, IMyAnimeListApiFactory malApiFactory)
        {
            m_finderFactory = finderFactory;
            m_malApiFactory = malApiFactory;
        }

        [HttpPost]
        public JsonResult Index([Required] AnimeRecsInputJson input)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
                // TODO: Log
                return Json(new { error = true });
            }

            using (IMyAnimeListApi malApi = m_malApiFactory.GetMalApi())
            using (IRecommendationFinder recFinder = m_finderFactory.GetRecommendationFinder())
            {
                ICollection<MyAnimeListEntry> animeList = malApi.GetAnimeListForUser(input.MalName);
                IGoodOkBadFilter goodOkBadFilter = input.GoodOkBadFilter;

                RecommendationResults results = recFinder.GetRecommendations(animeList, goodOkBadFilter);
                return Json(results);
            }
        }
    }
}
