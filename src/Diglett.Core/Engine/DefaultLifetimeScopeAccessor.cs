using Autofac;
using Autofac.Core.Lifetime;
using Diglett.Core.Threading;
using Microsoft.AspNetCore.Http;

namespace Diglett.Core.Engine
{
    public class DefaultLifetimeScopeAccessor : ILifetimeScopeAccessor
    {
        internal static readonly object ScopeTag = "CustomScope";

        private readonly ContextState<ILifetimeScope> _contextState;
        private readonly ILifetimeScope _rootContainer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultLifetimeScopeAccessor(IServiceProvider applicationServices, IHttpContextAccessor httpContextAccessor)
        {
            Guard.NotNull(applicationServices);
            Guard.NotNull(httpContextAccessor);

            _contextState = new ContextState<ILifetimeScope>("CustomLifetimeScopeProvider.WorkScope");
            _rootContainer = applicationServices.AsLifetimeScope();
            _httpContextAccessor = httpContextAccessor;
        }

        public ILifetimeScope LifetimeScope
        {
            get
            {
                var scope = _contextState.Get();
                if (scope == null)
                {
                    scope = _httpContextAccessor.HttpContext?.GetServiceScope();

                    if (scope != null)
                        scope.CurrentScopeEnding += OnScopeEnding;
                    else
                        scope = CreateLifetimeScope();

                    _contextState.Push(scope);
                }

                return scope;
            }
            set
            {
                _contextState.Push(value);
            }
        }

        public ILifetimeScope CreateLifetimeScope(Action<ContainerBuilder>? configurationAction = null)
        {
            var scope = (configurationAction == null)
                ? _rootContainer.BeginLifetimeScope(ScopeTag)
                : _rootContainer.BeginLifetimeScope(ScopeTag, configurationAction);

            scope.CurrentScopeEnding += OnScopeEnding;

            return scope;
        }

        private void OnScopeEnding(object? sender, LifetimeScopeEndingEventArgs args)
        {
            _contextState.Remove();
        }
    }
}
