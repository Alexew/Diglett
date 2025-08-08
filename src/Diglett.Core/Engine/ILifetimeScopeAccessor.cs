using Autofac;

namespace Diglett.Core.Engine
{
    public interface ILifetimeScopeAccessor
    {
        ILifetimeScope LifetimeScope { get; set; }

        ILifetimeScope CreateLifetimeScope(Action<ContainerBuilder>? configurationAction = null);
    }
}
