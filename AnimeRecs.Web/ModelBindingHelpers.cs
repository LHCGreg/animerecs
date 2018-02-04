using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnimeRecs.Web
{
    static class ModelBindingHelpers
    {
        public static string ConstructErrorString(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("Tried to construct an AjaxError with a valid ModelState.");
            }

            List<string> errorList = new List<string>();
            foreach (var x in modelState.SelectMany(p => p.Value.Errors.Select(e => new { Property = p.Key, ErrorMessage = e.ErrorMessage, Exception = e.Exception, RawValue = p.Value.RawValue })))
            {
                string errorMessage = !string.IsNullOrEmpty(x.ErrorMessage) ? x.ErrorMessage : x?.Exception?.Message;

                if (errorMessage != null && x.Property != null && x.RawValue != null)
                {
                    errorList.Add($"Error with property {x.Property}: {errorMessage} Raw value = {x.RawValue}");
                }
                else if (errorMessage != null && x.Property != null)
                {
                    errorList.Add($"Error with property {x.Property}: {errorMessage}");
                }
                else if (errorMessage != null)
                {
                    errorList.Add(errorMessage);
                }
                else if (x.Property != null)
                {
                    errorList.Add($"Error with property {x.Property}");
                }
                else
                {
                    errorList.Add("Unknown error.");
                }
            }
            return string.Join("\n\n", errorList);
        }
    }
}
