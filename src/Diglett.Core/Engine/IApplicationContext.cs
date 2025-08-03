using Autofac;

namespace Diglett.Core.Engine
{
    public interface IServiceProviderContainer
    {
        IServiceProvider? ApplicationServices { get; set; }
    }

    public interface IApplicationContext
    {
        ILifetimeScope Services { get; }
    }
}
