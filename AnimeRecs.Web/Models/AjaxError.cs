using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnimeRecs.Web.Models
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
