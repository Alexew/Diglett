using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Diglett
{
    public static class IServiceProviderExtensions
    {
        public static ILifetimeScope AsLifetimeScope(this IServiceProvider serviceProvider)
            => serviceProvider.GetAutofacRoot();
    }
}
