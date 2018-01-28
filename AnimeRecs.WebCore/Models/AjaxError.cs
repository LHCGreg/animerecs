using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnimeRecs.WebCore.Models
{
    /// <summary>
    /// Returned to the client on ajax calls that fail along with an error HTTP status code.
    /// This is only returned for certain expected errors.
    /// If an error HTTP status code is returned, check if the Content-Type contains application/json.
    /// You cannot check simple equality because it will actually be something like application/json; charset=utf-8.
    /// If so, the content is an AjaxError.
    /// If not, something went wrong, who knows what. Displaying a generic error message may be appropriate.
    /// </summary>
    public class AjaxError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public AjaxError()
        {
            ;
        }

        public AjaxError(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public AjaxError(ModelStateDictionary modelState)
        {
            ErrorCode = InvalidInput;
            Message = ModelBindingHelpers.ConstructErrorString(modelState);
        }

        public static string InvalidInput { get { return "InvalidInput"; } }
        public static string NoSuchMALUser { get { return "NoSuchMALUser"; } }
        public static string InternalError { get { return "InternalError"; } }
    }
}

// Copyright (C) 2018 Greg Najda
//
// This file is part of AnimeRecs.WebCore.
//
// AnimeRecs.WebCore is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.WebCore is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.WebCore.  If not, see <http://www.gnu.org/licenses/>.