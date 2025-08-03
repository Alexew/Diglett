namespace Diglett.Core.Engine
{
    public class EngineContext
    {
        private static IEngine _instance = default!;

        public static IEngine Current
        {
            get => _instance;
        }

        public static void Replace(IEngine engine)
        {
            Interlocked.Exchange(ref _instance, engine);
        }
    }
}
