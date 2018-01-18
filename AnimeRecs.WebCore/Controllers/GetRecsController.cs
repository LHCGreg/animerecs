using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.WebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnimeRecs.WebCore.Controllers
{
    public class GetRecsController : Controller
    {
        private ILogger _logger;

        public GetRecsController(ILogger<GetRecsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/api/GetRecs")]
        public Task<IActionResult> GetRecs([FromBody]AnimeRecsInputJson input/*,
            [FromServices]IOptionsSnapshot<Config.RecommendationsConfig> recConfig,
            [FromServices]IMyAnimeListApiFactory malApiFactory, [FromServices]IAnimeRecsClientFactory recClientFactory,
            [FromServices]IAnimeRecsDbConnectionFactory dbConnFactory*/)
        {
            if (!ModelState.IsValid)
            {
                AjaxError error = new AjaxError(ModelState);
                return Task.FromResult<IActionResult>(BadRequest(error));
            }

            return Task.FromResult<IActionResult>(Content("hello", "text/plain"));
        }
    }
}
