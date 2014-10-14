using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Responses.Negotiation;

namespace AnimeRecs.NancyWeb
{
    static class ErrorHandler
    {
        public static Response HandleException(NancyContext ctx, Exception ex, IResponseNegotiator responder)
        {
            LogException(ctx, ex);

            // Return AjaxError JSON if the client was making an AJAX request. Otherwise show an error page.
            var negotiator = new Negotiator(ctx);
            negotiator = negotiator.WithStatusCode(HttpStatusCode.InternalServerError)
                // Use this model as a JSON response if the client asked for application/json
                .WithMediaRangeModel("application/json", new AjaxError(AjaxError.InternalError, "Sorry, something went wrong when processing your request."))
                
                // Use this model as the model for the view if anything other than JSON
                .WithModel(new ErrorViewModel(ex))
                // Use this view for text/html
                .WithView("Error");
            return responder.NegotiateResponse(negotiator, ctx);
        }

        private static void LogException(NancyContext ctx, Exception ex)
        {
            string url = null;
            if (ctx != null && ctx.Request != null && ctx.Request.Url != null)
            {
                url = ctx.Request.Url.ToString();
            }
            Logging.Log.ErrorFormat("Error handling url {0}: {1}", ex, url, ex.Message);
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
