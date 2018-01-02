using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeRecs.WebCore.Models;
using Microsoft.Extensions.Options;

namespace AnimeRecs.WebCore.Controllers
{
    public class TestModel
    {
        public IOptions<Config.HtmlConfig> HtmlConfig { get; set; }
    }

    public class HomeController : Controller
    {
        public IActionResult Index(TestModel model)
        {
            Config.HtmlConfig htmlConfig = model.HtmlConfig.Value;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
