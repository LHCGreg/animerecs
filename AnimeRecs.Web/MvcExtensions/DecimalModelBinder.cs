using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimeRecs.Web.MvcExtensions
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        // Thanks to jaffia on stackoverflow: http://stackoverflow.com/questions/5500150/mvc3-model-binding-causes-the-parameter-conversion-from-type-system-int32-to
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return valueProviderResult == null ? base.BindModel(controllerContext, bindingContext) : Convert.ToDecimal(valueProviderResult.AttemptedValue);
        }
    }
}