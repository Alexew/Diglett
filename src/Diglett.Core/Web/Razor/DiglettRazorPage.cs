using Diglett.Core.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Diglett.Web.Razor
{
    public abstract class DiglettRazorPage<TModel> : RazorPage<TModel>
    {
        private IWebHelper? _webHelper;

        protected HttpRequest Request
        {
            get => Context.Request;
        }

        protected IWebHelper WebHelper => _webHelper ??= Context.RequestServices.GetRequiredService<IWebHelper>();

        protected T? Resolve<T>() where T : notnull
        {
            return Context.RequestServices.GetService<T>();
        }
    }
}
