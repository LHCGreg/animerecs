using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace AnimeRecs.Web.Controllers
{
    public class ErrorController : Controller
    {
        // For exceptions that reach the ASP.NET runtime
        public ActionResult Index()
        {
            return View();
        }

        // For errors from the runtime before a controller is entered such as a 404.
        public ActionResult Error(int statusCode, Exception exception)
        {
            string specificErrorViewName = "Error" + statusCode.ToString(CultureInfo.InvariantCulture);
            ViewEngineResult viewSearchResult = ViewEngines.Engines.FindView(ControllerContext, specificErrorViewName, masterName: null);
            if (viewSearchResult.View == null)
            {
                // return generic error view if no specific error view available
                return View();
            }

            return View(viewSearchResult.View);
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