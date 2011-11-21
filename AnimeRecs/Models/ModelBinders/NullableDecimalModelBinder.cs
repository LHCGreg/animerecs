using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimeRecs.Models
{
    public class NullableDecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null)
            {
                return (decimal?)null;
            }
            else
            {
                return (decimal?)(Convert.ToDecimal(valueProviderResult.AttemptedValue));
            }
        }
    }
}