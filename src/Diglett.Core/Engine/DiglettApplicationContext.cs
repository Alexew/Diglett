using Autofac;

namespace Diglett.Core.Engine
{
    public class DiglettApplicationContext : IApplicationContext, IServiceProviderContainer
    {
        IServiceProvider? IServiceProviderContainer.ApplicationServices { get; set; }
        public ILifetimeScope Services
        {
            get
            {
                var provider = ((IServiceProviderContainer)this).ApplicationServices ??
                    throw new InvalidOperationException("IServiceProviderContainer is null.");

                return provider.AsLifetimeScope();
            }
        }
    }
}
