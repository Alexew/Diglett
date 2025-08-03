using System.Runtime.CompilerServices;

namespace Diglett.Core.Engine
{
    public static class EngineFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create(IApplicationContext applicationContext)
        {
            if (EngineContext.Current == null)
            {
                EngineContext.Replace(new DiglettEngine(applicationContext));
            }

            return EngineContext.Current!;
        }
    }
}
