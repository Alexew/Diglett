using Autofac;

namespace Diglett.Core.Engine
{
    public interface IEngine
    {
        IApplicationContext Application { get; }
    }

    public static class IEngineExtensions
    {
        public static T Resolve<T>(this IEngine engine) where T : class
        {
            return engine.Application.Services.Resolve<T>();
        }
    }
}
