namespace Diglett.Core.Engine
{
    public class DiglettEngine : IEngine
    {
        public DiglettEngine(IApplicationContext applicationContext)
        {
            Application = applicationContext;
        }

        public IApplicationContext Application { get; }
    }
}
