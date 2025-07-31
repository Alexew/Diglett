namespace Diglett
{
    public abstract class Disposable : IDisposable
    {
        private const int DisposedFlag = 1;
        private int _isDisposed;

        public void Dispose()
        {
            var wasDisposed = Interlocked.Exchange(ref _isDisposed, DisposedFlag);
            if (wasDisposed == DisposedFlag)
                return;

            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose(bool disposing) { }
    }
}
