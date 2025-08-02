namespace Diglett.Core.Collections
{
    public interface IPagedList<T> : IPageable<T>
    {
        IPagedList<T> Load(bool force = false);
        Task<IPagedList<T>> LoadAsync(bool force = false, CancellationToken cancelToken = default);
        void Clear();
    }
}
