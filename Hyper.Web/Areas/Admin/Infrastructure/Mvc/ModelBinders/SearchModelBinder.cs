using Hyper.Web.Areas.Admin.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyper.Web.Areas.Admin.Infrastructure.Mvc.ModelBinders
{
    public class SearchModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var model = Activator.CreateInstance(bindingContext.ModelType);
                var pagingProperties = new List<string> { "PageSize", "PageIndex", "Draw" };
                var modelProperties = model.GetType().GetProperties().Where(x => !pagingProperties.Contains(x.Name));
                foreach (var property in modelProperties)
                {
                    property.SetValue(model, Convert.ChangeType(bindingContext.ValueProvider.GetValue(property.Name).FirstValue, property.PropertyType) );
                }
                var draw = bindingContext.ValueProvider.GetValue("draw").FirstValue;
                var start = bindingContext.ValueProvider.GetValue("start").FirstValue;
                var length = bindingContext.ValueProvider.GetValue("length").FirstValue;
                int pageSize = !string.IsNullOrEmpty(length) ? Convert.ToInt32(length) : 0;
                int pageIndex = !string.IsNullOrEmpty(start) ? (Convert.ToInt32(start) / pageSize) : 0;
                model.GetType().GetProperty("Draw").SetValue(model, Convert.ToInt32(draw));
                model.GetType().GetProperty("PageSize").SetValue(model, Convert.ToInt32(pageSize));
                model.GetType().GetProperty("PageIndex").SetValue(model, Convert.ToInt32(pageIndex));
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError("Error", "Received Model cannot be serialized");
                return null;
            }
        }
    }

    public class SearchModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType.BaseType == typeof(BaseSearchModel))
                return new SearchModelBinder();
            return null;
        }
    }
}
