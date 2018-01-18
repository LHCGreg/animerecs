using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeRecs.WebCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AnimeRecs.WebCore.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{code}")]
        public IActionResult Error(int statusCode)
        {
            // only 500 handled currently
            var exFeatInterface = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (exFeatInterface == null)
            {
                throw new Exception("No IExceptionHandlerFeature!");
            }

            ExceptionHandlerFeature exFeat = exFeatInterface as ExceptionHandlerFeature;
            if (exFeat != null && exFeat.Path != null && exFeat.Path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
            {
                return ApiError(statusCode, exFeatInterface.Error);
            }
            else
            {
                return HtmlError(statusCode, exFeatInterface.Error);
            }
        }

        private IActionResult ApiError(int statusCode, Exception ex)
        {
            AjaxError error = new AjaxError(AjaxError.InternalError, "Sorry, something went wrong when processing your request.");
            return StatusCode(statusCode, error);
        }

        private IActionResult HtmlError(int statusCode, Exception ex)
        {
            ErrorViewModel viewModel = new ErrorViewModel(ex);
            return View("Error", viewModel);
        }
    }
}
