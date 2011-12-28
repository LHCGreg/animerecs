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
        private double m_defaultBadPercentile;

        public FindBestMatchController()
            : this(ApplicationGlobals.RecommendationFinderFactory, ApplicationGlobals.MalApiFactory, GetDefaultGoodPercentile(), GetDefaultBadPercentile())
        {
            ;
        }

        private static double GetDefaultGoodPercentile()
        {
            return double.Parse(ConfigurationManager.AppSettings["DefaultGoodPercentile"], CultureInfo.InvariantCulture);
        }

        private static double GetDefaultBadPercentile()
        {
            return double.Parse(ConfigurationManager.AppSettings["DefaultBadPercentile"], CultureInfo.InvariantCulture);
        }
        
        public FindBestMatchController(IRecommendationFinderFactory finderFactory, IMyAnimeListApiFactory malApiFactory,
            double defaultGoodPercentile, double defaultBadPercentile)
        {
            m_finderFactory = finderFactory;
            m_malApiFactory = malApiFactory;
            m_defaultGoodPercentile = defaultGoodPercentile;
            m_defaultBadPercentile = defaultBadPercentile;
        }

        [HttpPost]
        public ActionResult Index([Required] AnimeRecsInputJson input)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
                // TODO: Log
                return new HttpStatusCodeResult(400);
            }

            using (IMyAnimeListApi malApi = m_malApiFactory.GetMalApi())
            using (IRecommendationFinder recFinder = m_finderFactory.GetRecommendationFinder())
            {
                ICollection<MyAnimeListEntry> animeList = malApi.GetAnimeListForUser(input.MalName);
                IGoodOkBadFilter goodOkBadFilter = GetGoodOkBadFilter(input);

                RecommendationResults results = recFinder.GetRecommendations(animeList, goodOkBadFilter);
                //return PartialView(results);

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
                resultsWithHtml.OkCutoff = results.OkCutoff;

                return Json(resultsWithHtml);
            }
        }

        private IGoodOkBadFilter GetGoodOkBadFilter(AnimeRecsInputJson input)
        {
            if (input.GoodCutoff.HasValue)
            {
                return new CutoffGoodOkBadFilter() { GoodCutoff = input.GoodCutoff.Value, OkCutoff = input.OkCutoff.Value };
            }
            else if (input.GoodPercentile.HasValue)
            {
                return new PercentileGoodOkBadFilter()
                {
                    RecommendedPercentile = decimal.ToDouble(input.GoodPercentile.Value),
                    DislikedPercentile = decimal.ToDouble(input.DislikedPercentile.Value)
                };
            }
            else
            {
                return new PercentileGoodOkBadFilter()
                {
                    RecommendedPercentile = m_defaultGoodPercentile,
                    DislikedPercentile = m_defaultBadPercentile
                };
            }
        }
    }
}
