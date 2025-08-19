using Diglett.Core.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Diglett.Web
{
    public class PartialInvoker : IPartialInvoker
    {
        private readonly ICompositeViewEngine _viewEngine;

        public PartialInvoker(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        public async Task<IHtmlContent> InvokeAsync(PartialContext context)
        {
            Guard.NotNull(context);

            using var writer = new StringWriter();

            var viewContext = CreateViewContext(context, writer, context.Model);
            var viewEngineResult = FindView(viewContext, context.ViewName, context.IsMainPage);

            viewEngineResult.EnsureSuccessful(originalLocations: null);

            var view = viewContext.View = viewEngineResult.View;

            using (view as IDisposable)
            {
                await view.RenderAsync(viewContext);
            }

            return new HtmlString(viewContext.Writer.ToString());
        }

        private ViewContext CreateViewContext(PartialContext context, TextWriter writer, object? model)
        {
            var services = context.HttpContext.RequestServices;
            var actionContext = context.ActionContext;

            var tempData = context.TempData ?? services.GetRequiredService<ITempDataDictionaryFactory>().GetTempData(context.HttpContext);

            var viewData = model != null || context.Model != null
                ? new ViewDataDictionary<object>(GetOrCreateViewData(), model ?? context.Model)
                : GetOrCreateViewData();

            var viewContext = new ViewContext(
                actionContext,
                NullView.Instance,
                viewData,
                tempData,
                writer,
                services.GetRequiredService<IOptions<MvcViewOptions>>().Value.HtmlHelperOptions);

            return viewContext;

            ViewDataDictionary GetOrCreateViewData()
            {
                return context.ViewData ??
                    new ViewDataDictionary(services.GetRequiredService<IModelMetadataProvider>(), actionContext.ModelState);
            }
        }

        private ViewEngineResult FindView(ActionContext actionContext, string viewName, bool isMainPage)
        {
            var result = _viewEngine.FindView(actionContext, viewName, isMainPage: isMainPage);
            var searchedLocations = result.SearchedLocations;

            if (!result.Success)
            {
                result = ViewEngineResult.NotFound(viewName, searchedLocations ?? result.SearchedLocations);
            }

            return result;
        }
    }
}
