namespace Diglett.Core.Engine
{
    public interface IEngine
    {
        IApplicationContext Application { get; }
        ScopedServiceContainer Scope { get; set; }
    }
}
