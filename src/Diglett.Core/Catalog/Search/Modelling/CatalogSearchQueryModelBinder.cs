using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Diglett.Core.Catalog.Search.Modelling
{
    public class CatalogSearchQueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Guard.NotNull(bindingContext);

            var factory = bindingContext.HttpContext.RequestServices.GetService<ICatalogSearchQueryFactory>();

            if (factory == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else if (factory.Current != null)
            {
                bindingContext.Result = ModelBindingResult.Success(factory.Current);
            }
            else if (bindingContext.ModelType != typeof(CatalogSearchQuery))
            {
                bindingContext.Result = ModelBindingResult.Success(new CatalogSearchQuery());
            }
            else
            {
                var query = factory.CreateFromQuery();

                if (query == null)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(query);
                }
            }

            return Task.CompletedTask;
        }
    }
}
