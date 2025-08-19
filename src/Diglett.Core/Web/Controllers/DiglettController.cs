using Diglett.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Diglett.Web.Controllers
{
    public abstract class DiglettController : Controller
    {
        protected Task<string> InvokePartialViewAsync(string viewName, object? model)
        {
            Guard.NotEmpty(viewName);

            return InvokeWidget(viewName, model, null);
        }

        protected Task<string> InvokePartialViewAsync(string viewName, ViewDataDictionary? viewData)
        {
            Guard.NotEmpty(viewName);

            return InvokeWidget(viewName, null, viewData);
        }

        private async Task<string> InvokeWidget(string viewName, object? model, ViewDataDictionary? viewData)
        {
            var context = new PartialContext(ControllerContext)
            {
                ViewName = viewName,
                Model = model,
                ViewData = viewData ?? ViewData,
                TempData = TempData
            };

            var invoker = context.HttpContext.RequestServices.GetRequiredService<IPartialInvoker>();

            return (await invoker.InvokeAsync(context)).ToHtmlString().ToString();
        }
    }
}
