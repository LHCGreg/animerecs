using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;
using MalApi;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;
using Newtonsoft.Json;

namespace AnimeRecs.NancyWeb.Modules.GetRecs
{
    public class GetRecsModule : NancyModule
    {
        private IConfig _config;
        private IMyAnimeListApiFactory _malApiFactory;
        private IAnimeRecsClientFactory _recClientFactory;
        private IAnimeRecsDbConnectionFactory _dbConnectionFactory;
        private IViewLocator _viewLocator;
        private RazorViewEngine _viewEngine;
        private IRenderContextFactory _renderContextFactory;

        public GetRecsModule(IConfig config, IMyAnimeListApiFactory malApiFactory, IAnimeRecsClientFactory recClientFactory, IAnimeRecsDbConnectionFactory dbConnectionFactory, IViewFactory viewFactory, IViewLocator viewLocator, RazorViewEngine viewEngine, IRenderContextFactory renderContextFactory)
        {
            _config = config;
            _malApiFactory = malApiFactory;
            _recClientFactory = recClientFactory;
            _dbConnectionFactory = dbConnectionFactory;
            _viewLocator = viewLocator;
            _viewEngine = viewEngine;
            _renderContextFactory = renderContextFactory;

            Post["/GetRecs"] = GetRecs;
        }

        private dynamic GetRecs(dynamic arg)
        {
            Logging.Log.Info("Started processing GetRecs request.");

            AnimeRecsInputJson input;
            try
            {
                input = this.BindAndValidate<AnimeRecsInputJson>();
            }
            catch (JsonSerializationException ex)
            {
                AjaxError error = new AjaxError(AjaxError.InvalidInput, ex.Message);
                return Response.AsJson(error, HttpStatusCode.BadRequest);
            }
            if (!ModelValidationResult.IsValid)
            {
                string errorString = ModelBindingHelpers.ConstructErrorString(ModelValidationResult.Errors);
                AjaxError error = new AjaxError(AjaxError.InvalidInput, errorString);
                return Response.AsJson(error, HttpStatusCode.BadRequest);
            }

            try
            {
                return DoGetRecs(input);
            }
            catch (ShortCircuitException earlyResponse)
            {
                return earlyResponse.Response;
            }
        }

        private Response DoGetRecs(AnimeRecsInputJson input)
        {
            if (input.RecSourceName == null)
            {
                input.RecSourceName = _config.DefaultRecSource;
            }

            MalUserLookupResults userLookup = GetUserLookup(input);

            Dictionary<int, MalListEntry> animeList = new Dictionary<int, MalListEntry>();
            foreach (MyAnimeListEntry listEntry in userLookup.AnimeList)
            {
                animeList[listEntry.AnimeInfo.AnimeId] = new AnimeRecs.RecEngine.MAL.MalListEntry((byte?)listEntry.Score, listEntry.Status, (short)listEntry.NumEpisodesWatched);
            }

            Dictionary<int, MalListEntry> animeWithheld = WithholdAnime(input, animeList);

            MalRecResults<IEnumerable<IRecommendation>> recResults = GetRecommendations(input, animeList, animeWithheld);

            GetRecsViewModel viewModel = new GetRecsViewModel(
                results: recResults,
                userId: userLookup.UserId,
                userName: userLookup.CanonicalUserName,
                userLookup: userLookup,
                userAnimeList: animeList,
                maximumRecommendationsToReturn: _config.MaximumRecommendationsToReturn,
                animeWithheld: animeWithheld,
                dbConnectionFactory: _dbConnectionFactory
            );

            RecResultsAsHtmlJson resultsJson = GetResultHtml(viewModel, input);
            return Response.AsJson(resultsJson);

            //RecResultsAsHtml resultsHtml = new RecResultsAsHtml("");
            //return Response.AsJson(resultsHtml);
        }

        private MalUserLookupResults GetUserLookup(AnimeRecsInputJson input)
        {
            using (IMyAnimeListApi malApi = _malApiFactory.GetMalApi())
            {
                Logging.Log.InfoFormat("Getting MAL list for user {0}.", input.MalName);
                MalUserLookupResults userLookup;
                try
                {
                    userLookup = malApi.GetAnimeListForUser(input.MalName);
                    Logging.Log.InfoFormat("Got MAL list for user {0}.", input.MalName);
                    return userLookup;
                }
                catch (MalUserNotFoundException)
                {
                    Logging.Log.InfoFormat("User {0} not found.", input.MalName);
                    AjaxError error = new AjaxError(AjaxError.NoSuchMALUser, "No such MAL user.");
                    Response response = Response.AsJson(error, HttpStatusCode.BadRequest);
                    throw new ShortCircuitException(response);
                }
            }
        }

        // Removes anime to withhold from animeList and returns the withheld anime. This is only used in debug mode
        private Dictionary<int, MalListEntry> WithholdAnime(AnimeRecsInputJson input, Dictionary<int, MalListEntry> animeList)
        {
            Dictionary<int, MalListEntry> animeWithheld = new Dictionary<int, MalListEntry>();

            foreach (int animeIdToWithhold in input.AnimeIdsToWithhold)
            {
                if (animeList.ContainsKey(animeIdToWithhold))
                {
                    animeWithheld[animeIdToWithhold] = animeList[animeIdToWithhold];
                    animeList.Remove(animeIdToWithhold);
                }
            }

            if (input.PercentOfAnimeToWithhold > 0m)
            {
                int numAnimesToWithhold = (int)(animeList.Count * (input.PercentOfAnimeToWithhold / 100));
                Random rng = new Random();
                List<int> animeIdsToWithhold = animeList.Keys.OrderBy(animeId => rng.Next()).Take(numAnimesToWithhold).ToList();
                foreach (int animeIdToWithhold in animeIdsToWithhold)
                {
                    animeWithheld[animeIdToWithhold] = animeList[animeIdToWithhold];
                    animeList.Remove(animeIdToWithhold);
                }
            }

            return animeWithheld;
        }

        private MalRecResults<IEnumerable<IRecommendation>> GetRecommendations(AnimeRecsInputJson input, Dictionary<int, MalListEntry> animeList, Dictionary<int, MalListEntry> animeWithheld)
        {
            int numRecsToTryToGet = _config.MaximumRecommendationsToReturn;
            if (animeWithheld.Count > 0)
            {
                // Get rating prediction information about all anime if in debug mode and withholding anime.
                // For all currently implemented algorithms, this does not cause a performance problem.
                numRecsToTryToGet = 100000;
            }

            using (AnimeRecsClient recClient = _recClientFactory.GetClient(input.RecSourceName))
            {
                MalRecResults<IEnumerable<IRecommendation>> recResults;
                try
                {
                    if (input.GoodPercentile != null)
                    {
                        Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using target of top {3}%.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, input.GoodPercentile.Value);
                        recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, numRecsToTryToGet,
                            input.GoodPercentile.Value);
                    }
                    else if (input.GoodCutoff != null)
                    {
                        Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using target of {3}.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, input.GoodCutoff.Value);
                        recResults = recClient.GetMalRecommendations(animeList, input.RecSourceName, numRecsToTryToGet,
                            input.GoodCutoff.Value);
                    }
                    else
                    {
                        Logging.Log.InfoFormat("Querying rec source {0} for {1} recommendations for {2} using default target of top {3}%.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, AppGlobals.Config.DefaultTargetPercentile);
                        recResults = recClient.GetMalRecommendationsWithPercentileTarget(animeList, input.RecSourceName, numRecsToTryToGet,
                            AppGlobals.Config.DefaultTargetPercentile);
                    }
                }
                catch (AnimeRecs.RecService.DTO.RecServiceErrorException ex)
                {
                    if (ex.Error.ErrorCode == AnimeRecs.RecService.DTO.ErrorCodes.Maintenance)
                    {
                        Logging.Log.InfoFormat("Could not service recommendation request for {0}. The rec service is currently undergoing maintenance.",
                            input.MalName);
                        AjaxError error = new AjaxError(AjaxError.InternalError, "The site is currently undergoing scheduled maintenance. Check back in a few minutes.");
                        Response response = Response.AsJson(error, HttpStatusCode.InternalServerError);
                        throw new ShortCircuitException(response);
                    }
                    else
                    {
                        throw;
                    }
                }
                Logging.Log.InfoFormat("Got results from rec service for {0}.", input.MalName);

                return recResults;
            }
        }

        private RecResultsAsHtmlJson GetResultHtml(GetRecsViewModel viewModel, AnimeRecsInputJson input)
        {
            // Support detailed results for the AnimeRecs recommendation type
            string viewName;
            if (viewModel.Results.RecommendationType.Equals(AnimeRecs.RecService.DTO.RecommendationTypes.AnimeRecs) && input.DisplayDetailedResults)
            {
                viewName = viewModel.Results.RecommendationType + "_complex";
            }
            else
            {
                viewName = viewModel.Results.RecommendationType;
            }

            // Try to use a view specific for the recommendation type, fall back to the generic view if no specific view exists.
            string viewPath = "Modules/GetRecs/" + viewName;
            ViewLocationResult viewLocation = _viewLocator.LocateView(viewPath, this.Context);
            if (viewLocation == null)
            {
                viewPath = "Modules/GetRecs/Fallback";
                viewLocation = _viewLocator.LocateView(viewPath, this.Context);
            }

            // Render view to string
            ViewLocationContext locationContext = new ViewLocationContext() { Context = this.Context, ModulePath = this.ModulePath, ModuleName = "GetRecs" };
            IRenderContext renderContext = _renderContextFactory.GetRenderContext(locationContext);
            using (Response renderedView = _viewEngine.RenderView(viewLocation, viewModel, renderContext, isPartial: true))
            using (MemoryStream stream = new MemoryStream())
            {
                // Write rendered view context to memory stream
                renderedView.Contents(stream);
                // Read contents from memory stream
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string renderedHtml = reader.ReadToEnd();
                    return new RecResultsAsHtmlJson(renderedHtml);
                }
            }
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.NancyWeb.
//
// AnimeRecs.NancyWeb is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.NancyWeb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.NancyWeb.  If not, see <http://www.gnu.org/licenses/>.