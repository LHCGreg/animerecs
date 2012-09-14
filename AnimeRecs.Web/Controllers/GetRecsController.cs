using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AnimeRecs.Web.Models.Json;
using MalApi;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecEngine;
using AnimeRecs.Web.Models.ViewModels;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.DAL;

namespace AnimeRecs.Web.Controllers
{
    public class GetRecsController : Controller
    {
        private IMyAnimeListApiFactory m_malApiFactory;
        private IAnimeRecsClientFactory m_recClientFactory;
        private IAnimeRecsDbConnectionFactory m_dbConnectionFactory;
        
        public GetRecsController()
            : this(AppGlobals.MalApiFactory, AppGlobals.RecClientFactory, AppGlobals.DbConnectionFactory)
        {
            ;
        }

        public GetRecsController(IMyAnimeListApiFactory malApiFactory, IAnimeRecsClientFactory recClientFactory,
            IAnimeRecsDbConnectionFactory dbConnectionFactory)
        {
            m_malApiFactory = malApiFactory;
            m_recClientFactory = recClientFactory;
            m_dbConnectionFactory = dbConnectionFactory;
        }

        [HttpPost]
        public ActionResult Index(AnimeRecsInputJson input)
        {
            Logging.Log.Info("Started processing GetRecs request.");

            // Can't have [Required] on input because Mono's RequiredAttribute is not permitted on parameters.
            // Fixed in Mono trunk - https://bugzilla.xamarin.com/show_bug.cgi?id=6230
            // But it'll be some time before you can count on that fix being in Mono installations. :(
            if (input == null || !ModelState.IsValid)
            {
                Logging.Log.Info("Invalid input.");
                return new HttpStatusCodeResult(400);
            }

            using (IMyAnimeListApi malApi = m_malApiFactory.GetMalApi())
            {
                Logging.Log.InfoFormat("Getting MAL list for user {0}.", input.MalName);
                MalUserLookupResults userLookup;
                try
                {
                    userLookup = malApi.GetAnimeListForUser(input.MalName);
                    Logging.Log.InfoFormat("Got MAL list for user {0}.", input.MalName);
                }
                catch (MalUserNotFoundException)
                {
                    Logging.Log.InfoFormat("User {0} not found.", input.MalName);
                    AjaxError error = new AjaxError("No such MAL user.");
                    Response.StatusCode = 500; // internal server error
                    return Json(error);
                }

                Dictionary<int, MalListEntry> animeList = new Dictionary<int, MalListEntry>();

                foreach (MyAnimeListEntry listEntry in userLookup.AnimeList)
                {
                    animeList[listEntry.AnimeInfo.AnimeId] = new RecEngine.MAL.MalListEntry((byte?)listEntry.Score, listEntry.Status, (short)listEntry.NumEpisodesWatched);
                }

                try
                {
                    using (AnimeRecsClient recClient = m_recClientFactory.GetClient(input.RecSourceName))
                    {
                        MalRecResults<IEnumerable<IRecommendation>> recResults;
                        if (input.GoodPercentile != null)
                        {
                            Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using target of top {3}%.",
                                input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn, input.MalName, input.GoodPercentile.Value);
                            recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                                input.GoodPercentile.Value);
                        }
                        else if (input.GoodCutoff != null)
                        {
                            Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using target of {3}.",
                                input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn, input.MalName, input.GoodCutoff.Value);
                            recResults = recClient.GetMalRecommendations(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                                input.GoodCutoff.Value);
                        }
                        else
                        {
                            Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using default target of top {3}%.",
                                input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn, input.MalName, AppGlobals.Config.DefaultTargetPercentile);
                            recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, AppGlobals.Config.MaximumRecommendationsToReturn,
                                AppGlobals.Config.DefaultTargetPercentile);
                        }

                        Logging.Log.InfoFormat("Got results from rec service for {0}.", input.MalName);

                        string viewSuffix = null;
                        if (recResults.RecommendationType.Equals(AnimeRecs.RecService.DTO.RecommendationTypes.AnimeRecs) && input.DisplayDetailedResults)
                        {
                            viewSuffix = "complex";
                        }

                        RecResultsAsHtml resultsJson = ResultsToReturnValue(recResults, userLookup.UserId, userLookup.CanonicalUserName, animeList, viewSuffix);
                        Logging.Log.Debug("Converted results to return value.");

                        return Json(resultsJson);
                    }
                }
                catch (AnimeRecs.RecService.DTO.RecServiceErrorException ex)
                {
                    if (ex.Error.ErrorCode == AnimeRecs.RecService.DTO.ErrorCodes.Maintenance)
                    {
                        Logging.Log.Info("Could not service recommendation request for {0}. The rec service is currently undergoing maintenance.");
                        AjaxError error = new AjaxError("The site is currently undergoing scheduled maintenance. Check back in a few minutes.");
                        Response.StatusCode = 500;
                        return Json(error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private RecResultsAsHtml ResultsToReturnValue(MalRecResults<IEnumerable<IRecommendation>> basicResults, int userId,
            string userName, IDictionary<int, MalListEntry> userAnimeList, string viewSuffix)
        {
            string viewName;
            if (viewSuffix == null)
            {
                viewName = basicResults.RecommendationType;
            }
            else
            {
                viewName = basicResults.RecommendationType + "_" + viewSuffix;
            }
            
            ViewEngineResult viewSearchResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
            if (viewSearchResult.View == null)
            {
                var viewModel = new GetRecsViewModel(basicResults, userId, userName, userAnimeList, m_dbConnectionFactory);
                viewSearchResult = ViewEngines.Engines.FindPartialView(ControllerContext, "Fallback");
            }

            using (StringWriter htmlWriter = new StringWriter())
            {
                ViewData.Model = new GetRecsViewModel(basicResults, userId, userName, userAnimeList, m_dbConnectionFactory);
                ViewContext viewContext = new ViewContext(ControllerContext, viewSearchResult.View, ViewData, TempData, htmlWriter);
                viewSearchResult.View.Render(viewContext, htmlWriter);

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