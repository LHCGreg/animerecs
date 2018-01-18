using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeRecs.WebCore.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AnimeRecs.WebCore.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Testing logging");
            throw new Exception("Error error error!");
            return View();
        }
    }
}
