using Autofac;
using Microsoft.AspNetCore.Http;

namespace Diglett
{
    public static class HttpContextExtensions
    {
        public static ILifetimeScope GetServiceScope(this HttpContext httpContext)
        {
            return httpContext.RequestServices.AsLifetimeScope();
        }
    }
}
