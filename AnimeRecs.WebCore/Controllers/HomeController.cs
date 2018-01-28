using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeRecs.WebCore.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using AnimeRecs.RecService.ClientLib;
using System.Threading;

namespace AnimeRecs.WebCore.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery]string algorithm, [FromQuery]bool? detailedResults, [FromQuery]bool? debugMode,
            [FromServices]IOptionsSnapshot<Config.RecommendationsConfig> recConfig, [FromServices]IAnimeRecsClientFactory recClientFactory)
        {
            if (!ModelState.IsValid)
            {
                string errorString = ModelBindingHelpers.ConstructErrorString(ModelState);
                _logger.LogDebug("Invalid input received for home page: {0}", errorString);
                return View("Error", new ErrorViewModel(exception: null));
            }

            algorithm = algorithm ?? recConfig.Value.DefaultRecSource;
            bool displayDetailedResults = detailedResults ?? false;
            bool debugModeOn = debugMode ?? false;

            string recSourceType = null;

            using (AnimeRecsClient client = recClientFactory.GetClient(algorithm))
            {
                try
                {
                    recSourceType = await client.GetRecSourceTypeAsync(algorithm,
                        TimeSpan.FromMilliseconds(recConfig.Value.TimeoutMilliseconds), CancellationToken.None);
                }
                catch
                {
                    ;
                }
            }

            bool algorithmAvailable = recSourceType != null;

            bool targetScoreNeeded = false;
            if (AnimeRecs.RecService.DTO.RecSourceTypes.AnimeRecs.Equals(recSourceType, StringComparison.OrdinalIgnoreCase) && displayDetailedResults)
            {
                targetScoreNeeded = true;
            }

            HomeViewModel viewModel = new HomeViewModel(
                algorithm: algorithm,
                algorithmAvailable: algorithmAvailable,
                targetScoreNeeded: targetScoreNeeded,
                displayDetailedResults: displayDetailedResults,
                debugModeOn: debugModeOn
            );


            return View(viewModel);
        }
    }
}
