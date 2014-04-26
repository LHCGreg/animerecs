using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimeRecs.Web.MvcExtensions
{
    public class LoggingExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string url = null;
            // Not sure if any of this can be null
            if (filterContext.HttpContext != null && filterContext.HttpContext.Request != null && filterContext.HttpContext.Request.RawUrl != null)
            {
                url = filterContext.HttpContext.Request.Url.ToString();
            }

            // Not sure if this can be null
            if (filterContext.Exception != null)
            {
                Logging.Log.ErrorFormat("{0} : {1}", filterContext.Exception, url, filterContext.Exception.Message);
            }
            else
            {
                Logging.Log.ErrorFormat("{0} : {1}", url, "Unknown error");
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