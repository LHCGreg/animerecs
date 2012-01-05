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
using System.Globalization;
using System.IO;

namespace AnimeRecs.Controllers
{
    public class FindBestMatchController : Controller
    {
        private IRecommendationFinderFactory m_finderFactory;
        private IMyAnimeListApiFactory m_malApiFactory;
        private double m_defaultGoodPercentile;

        public FindBestMatchController()
            : this(AppGlobals.RecommendationFinderFactory, AppGlobals.MalApiFactory, (double)AppGlobals.Config.DefaultLikedPercent)
        {
            ;
        }
        
        public FindBestMatchController(IRecommendationFinderFactory finderFactory, IMyAnimeListApiFactory malApiFactory,
            double defaultGoodPercentile)
        {
            m_finderFactory = finderFactory;
            m_malApiFactory = malApiFactory;
            m_defaultGoodPercentile = defaultGoodPercentile;
        }

        [HttpPost]
        public ActionResult Index([Required] AnimeRecsInputJson input)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Log
                return new HttpStatusCodeResult(400);
            }

            using (IMyAnimeListApi malApi = m_malApiFactory.GetMalApi())
            using (IRecommendationFinder recFinder = m_finderFactory.GetRecommendationFinder())
            {
                ICollection<MyAnimeListEntry> animeList = malApi.GetAnimeListForUser(input.MalName);
                IGoodOkBadFilter goodOkBadFilter = GetGoodOkBadFilter(input);

                RecommendationResults results = recFinder.GetRecommendations(animeList, goodOkBadFilter);

                RecommendationResultsWithHtml resultsWithHtml = new RecommendationResultsWithHtml();

                using (StringWriter htmlWriter = new StringWriter())
                {
                    ViewData.Model = results;
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "Index");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, htmlWriter);
                    viewResult.View.Render(viewContext, htmlWriter);

                    resultsWithHtml.Html = htmlWriter.ToString();
                }

                resultsWithHtml.RecommendedCutoff = results.RecommendedCutoff;

                return Json(resultsWithHtml);
            }
        }

        private IGoodOkBadFilter GetGoodOkBadFilter(AnimeRecsInputJson input)
        {
            // The distinction between "disliked" and "ok" doesn't matter, all we care about is "liked" and "not liked".
            if (input.GoodCutoff.HasValue)
            {
                return new CutoffGoodOkBadFilter() { GoodCutoff = input.GoodCutoff.Value, OkCutoff = input.GoodCutoff.Value - 1 };
            }
            else if (input.GoodPercentile.HasValue)
            {
                return new PercentileGoodOkBadFilter()
                {
                    RecommendedPercentile = decimal.ToDouble(input.GoodPercentile.Value),
                    DislikedPercentile = 0
                };
            }
            else
            {
                return new PercentileGoodOkBadFilter()
                {
                    RecommendedPercentile = m_defaultGoodPercentile,
                    DislikedPercentile = 0
                };
            }
        }
    }
}
