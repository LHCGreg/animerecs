using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AnimeRecs.Web.Models.Json;
using AnimeRecs.MalApi;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecEngine;
using AnimeRecs.Web.Models.ViewModels;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.Web.Controllers
{
    public class GetRecsController : Controller
    {
        private IMyAnimeListApiFactory m_malApiFactory;
        private IAnimeRecsClientFactory m_recClientFactory;
        
        public GetRecsController()
            : this(AppGlobals.MalApiFactory, AppGlobals.RecClientFactory)
        {
            ;
        }

        public GetRecsController(IMyAnimeListApiFactory malApiFactory, IAnimeRecsClientFactory recClientFactory)
        {
            m_malApiFactory = malApiFactory;
            m_recClientFactory = recClientFactory;
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
            {
                MalUserLookupResults userLookup;
                try
                {
                    userLookup = malApi.GetAnimeListForUser(input.MalName);
                }
                catch (MalUserNotFoundException)
                {
                    AjaxError error = new AjaxError("No such MAL user.");
                    Response.StatusCode = 500; // internal server error
                    return Json(error);
                }

                Dictionary<int, MalListEntry> animeList = new Dictionary<int, MalListEntry>();

                foreach (MyAnimeListEntry listEntry in userLookup.AnimeList)
                {
                    animeList[listEntry.AnimeInfo.AnimeId] = new RecEngine.MAL.MalListEntry(listEntry.Score, listEntry.Status, listEntry.NumEpisodesWatched);
                }

                using (AnimeRecsClient recClient = m_recClientFactory.GetClient())
                {
                    MalRecResults<IEnumerable<IRecommendation>> recResults;
                    if (input.GoodPercentile != null)
                    {
                        recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                            input.GoodPercentile.Value);
                    }
                    else if (input.GoodCutoff != null)
                    {
                        recResults = recClient.GetMalRecommendations(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                            input.GoodCutoff.Value);
                    }
                    else
                    {
                        recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                            AppGlobals.Config.DefaultTargetPercentile);
                    }

                    RecResultsAsHtml resultsJson = ResultsToReturnValue(recResults, animeList);
                    return Json(resultsJson);
                }
            }
        }

        private RecResultsAsHtml ResultsToReturnValue(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> userAnimeList)
        {
            if (basicResults.AsAverageScoreResults() != null)
            {
                var viewModel = new GetRecsViewModel<IEnumerable<AverageScoreRecommendation>>(basicResults.AsAverageScoreResults(), userAnimeList);
                return RenderResults(viewModel, "AverageScore");
            }
            else if (basicResults.AsMostPopularResults() != null)
            {
                var viewModel = new GetRecsViewModel<IEnumerable<MostPopularRecommendation>>(basicResults.AsMostPopularResults(), userAnimeList);
                return RenderResults(viewModel, "MostPopular");
            }
            else if (basicResults.AsRatingPredictionResults() != null)
            {
                var viewModel = new GetRecsViewModel<IEnumerable<RatingPredictionRecommendation>>(basicResults.AsRatingPredictionResults(), userAnimeList);
                return RenderResults(viewModel, "RatingPrediction");
            }
            else if (basicResults.AsAnimeRecsResults() != null)
            {
                var viewModel = new GetRecsViewModel<MalAnimeRecsResults>(basicResults.AsAnimeRecsResults(), userAnimeList);
                return RenderResults(viewModel, "AnimeRecs");
            }
            else
            {
                var viewModel = new GetRecsViewModel<IEnumerable<IRecommendation>>(basicResults, userAnimeList);
                return RenderResults(viewModel, "Fallback");
            }
        }

        private RecResultsAsHtml RenderResults(object malRecResults, string viewName)
        {
            using (StringWriter htmlWriter = new StringWriter())
            {
                ViewData.Model = malRecResults;
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, htmlWriter);
                viewResult.View.Render(viewContext, htmlWriter);

                return new RecResultsAsHtml(htmlWriter.ToString());
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.