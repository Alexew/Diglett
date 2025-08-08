using Autofac;

namespace Diglett.Core.Engine
{
    public sealed class ScopedServiceContainer
    {
        private readonly ILifetimeScopeAccessor _scopeAccessor;
        private readonly ILifetimeScope _rootContainer;

        public ScopedServiceContainer(
            ILifetimeScopeAccessor scopeAccessor,
            ILifetimeScope rootContainer)
        {
            Guard.NotNull(scopeAccessor);
            Guard.NotNull(rootContainer);

            _scopeAccessor = scopeAccessor;
            _rootContainer = rootContainer;
        }

        public ILifetimeScope RequestContainer => _scopeAccessor.LifetimeScope ?? _rootContainer;

        public T Resolve<T>() where T : class
            => RequestContainer.Resolve<T>();

        public T? ResolveOptional<T>() where T : class
            => RequestContainer.ResolveOptional<T>();
    }
}
