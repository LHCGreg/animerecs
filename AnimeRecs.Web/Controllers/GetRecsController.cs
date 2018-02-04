using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.Web.Models;
using MalApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnimeRecs.Web.Controllers
{
    public class GetRecsController : Controller
    {
        private ILogger _logger;

        public GetRecsController(ILogger<GetRecsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/api/GetRecs")]
        public async Task<IActionResult> GetRecs([FromBody]AnimeRecsInputJson input,
            [FromServices]IOptionsSnapshot<Config.RecommendationsConfig> recConfig,
            [FromServices]IMyAnimeListApiFactory malApiFactory, [FromServices]IAnimeRecsClientFactory recClientFactory,
            [FromServices]IAnimeRecsDbConnectionFactory dbConnFactory, [FromServices]IRazorViewEngine viewEngine,
            [FromServices]ITempDataProvider tempProvider)
        {
            if (!ModelState.IsValid)
            {
                AjaxError error = new AjaxError(ModelState);
                _logger.LogDebug("Invalid input received for GetRecs: {0}", error.Message);
                return BadRequest(error);
            }

            if (input.RecSourceName == null)
            {
                input.RecSourceName = recConfig.Value.DefaultRecSource;
            }

            try
            {
                MalUserLookupResults userLookup = await GetUserLookupAsync(input, malApiFactory).ConfigureAwait(false);

                Dictionary<int, MalListEntry> animeList = new Dictionary<int, MalListEntry>();
                foreach (MyAnimeListEntry listEntry in userLookup.AnimeList)
                {
                    animeList[listEntry.AnimeInfo.AnimeId] = new AnimeRecs.RecEngine.MAL.MalListEntry((byte?)listEntry.Score, listEntry.Status, (short)listEntry.NumEpisodesWatched);
                }

                Dictionary<int, MalListEntry> animeWithheld = WithholdAnime(input, animeList);

                MalRecResults<IEnumerable<IRecommendation>> recResults = await GetRecommendationsAsync(input, recConfig.Value, animeList, animeWithheld, recClientFactory).ConfigureAwait(false);

                GetRecsViewModel viewModel = new GetRecsViewModel(
                    results: recResults,
                    userId: userLookup.UserId,
                    userName: userLookup.CanonicalUserName,
                    userLookup: userLookup,
                    userAnimeList: animeList,
                    maximumRecommendationsToReturn: recConfig.Value.MaximumRecommendationsToReturn,
                    maximumRecommendersToReturn: recConfig.Value.MaximumRecommendersToReturn,
                    animeWithheld: animeWithheld,
                    dbConnectionFactory: dbConnFactory
                );

                RecResultsAsHtmlJson resultsJson = await GetResultHtmlAsync(viewModel, input, viewEngine, tempProvider).ConfigureAwait(false);

                return Ok(resultsJson);
            }
            catch (ShortCircuitException ex)
            {
                return ex.Result;
            }
        }

        private async Task<MalUserLookupResults> GetUserLookupAsync(AnimeRecsInputJson input, IMyAnimeListApiFactory malApiFactory)
        {
            using (IMyAnimeListApi malApi = malApiFactory.GetMalApi())
            {
                _logger.LogInformation("Getting MAL list for user {0}.", input.MalName);
                MalUserLookupResults userLookup;
                try
                {
                    userLookup = await malApi.GetAnimeListForUserAsync(input.MalName).ConfigureAwait(false);
                    _logger.LogInformation("Got MAL list for user {0}.", input.MalName);
                    return userLookup;
                }
                catch (MalUserNotFoundException)
                {
                    _logger.LogInformation("User {0} not found.", input.MalName);
                    AjaxError error = new AjaxError(AjaxError.NoSuchMALUser, "No such MAL user.");
                    JsonResult result = Json(error);
                    result.StatusCode = 404;
                    throw new ShortCircuitException(result);
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

        private async Task<MalRecResults<IEnumerable<IRecommendation>>> GetRecommendationsAsync(AnimeRecsInputJson input,
            Config.RecommendationsConfig recConfig, Dictionary<int, MalListEntry> animeList,
            Dictionary<int, MalListEntry> animeWithheld, IAnimeRecsClientFactory recClientFactory)
        {
            int numRecsToTryToGet = recConfig.MaximumRecommendationsToReturn;
            if (animeWithheld.Count > 0)
            {
                // Get rating prediction information about all anime if in debug mode and withholding anime.
                // For all currently implemented algorithms, this does not cause a performance problem.
                numRecsToTryToGet = 100000;
            }

            using (AnimeRecsClient recClient = recClientFactory.GetClient(input.RecSourceName))
            {
                MalRecResults<IEnumerable<IRecommendation>> recResults;
                try
                {
                    if (input.GoodPercentile != null)
                    {
                        decimal targetFraction = input.GoodPercentile.Value / 100;
                        _logger.LogInformation("Querying rec source {0} for {1} recommendations for {2} using target of top {3}%.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, targetFraction);
                        recResults = await recClient.GetMalRecommendationsWithFractionTargetAsync(animeList,
                            input.RecSourceName, numRecsToTryToGet, targetFraction,
                            TimeSpan.FromMilliseconds(recConfig.TimeoutMilliseconds), CancellationToken.None).ConfigureAwait(false);
                    }
                    else if (input.GoodCutoff != null)
                    {
                        _logger.LogInformation("Querying rec source {0} for {1} recommendations for {2} using target of {3}.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, input.GoodCutoff.Value);
                        recResults = await recClient.GetMalRecommendationsAsync(animeList, input.RecSourceName,
                            numRecsToTryToGet, input.GoodCutoff.Value, TimeSpan.FromMilliseconds(recConfig.TimeoutMilliseconds),
                            CancellationToken.None).ConfigureAwait(false);
                    }
                    else
                    {
                        decimal targetFraction = recConfig.DefaultTargetPercentile / 100;
                        _logger.LogInformation("Querying rec source {0} for {1} recommendations for {2} using default target of top {3}%.",
                            input.RecSourceName, numRecsToTryToGet, input.MalName, targetFraction);
                        recResults = await recClient.GetMalRecommendationsWithFractionTargetAsync(animeList,
                            input.RecSourceName, numRecsToTryToGet, targetFraction,
                            TimeSpan.FromMilliseconds(recConfig.TimeoutMilliseconds), CancellationToken.None)
                            .ConfigureAwait(false);
                    }
                }
                catch (AnimeRecs.RecService.DTO.RecServiceErrorException ex)
                {
                    if (ex.Error.ErrorCode == AnimeRecs.RecService.DTO.ErrorCodes.Maintenance)
                    {
                        _logger.LogInformation("Could not service recommendation request for {0}. The rec service is currently undergoing maintenance.",
                            input.MalName);
                        AjaxError error = new AjaxError(AjaxError.InternalError, "The site is currently undergoing scheduled maintenance. Check back in a few minutes.");
                        JsonResult result = Json(error);
                        result.StatusCode = 500;
                        throw new ShortCircuitException(result);
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.LogInformation("Got results from rec service for {0}.", input.MalName);

                return recResults;
            }
        }

        private async Task<RecResultsAsHtmlJson> GetResultHtmlAsync(GetRecsViewModel viewModel, AnimeRecsInputJson input,
            IRazorViewEngine viewEngine, ITempDataProvider tempProvider)
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

            IView view;
            ViewEngineResult viewSearchResult = viewEngine.FindView(ControllerContext, viewName, isMainPage: false);

            // Try to use a view specific for the recommendation type, fall back to the generic view if no specific view exists.
            if (viewSearchResult.Success)
            {
                view = viewSearchResult.View;
            }
            else
            {
                string fallbackViewName = "Fallback";
                ViewEngineResult fallbackViewSearchResult = viewEngine.FindView(ControllerContext, fallbackViewName, isMainPage: false);
                if (!fallbackViewSearchResult.Success)
                {
                    throw new Exception($"Failed to find fallback view named {fallbackViewName}.");
                }
                view = fallbackViewSearchResult.View;
            }

            // Render the view to a string
            using (StringWriter writer = new StringWriter())
            {
                ViewDataDictionary viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = viewModel
                };

                TempDataDictionary tempDataDictionary = new TempDataDictionary(HttpContext, tempProvider);

                ViewContext viewcontext = new ViewContext(ControllerContext, view, viewDictionary, tempDataDictionary,
                   writer, new HtmlHelperOptions());

                await view.RenderAsync(viewcontext).ConfigureAwait(false);
                string renderedHtml = writer.ToString();
                return new RecResultsAsHtmlJson(renderedHtml);
            }
        }
    }
}
