namespace Diglett.Core.Collections
{
    public class PagedList<T> : Pageable<T>, IPagedList<T>
    {
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
            : base(source, pageIndex, pageSize) { }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
            : base(source.AsQueryable(), pageIndex, pageSize, totalCount) { }

        public IPagedList<T> Load(bool force = false)
        {
            if (force && List != null)
            {
                List.Clear();
                List = null;
            }

            EnsureIsLoaded();

            return this;
        }

        public async Task<IPagedList<T>> LoadAsync(bool force = false, CancellationToken cancelToken = default)
        {
            if (force && List != null)
            {
                List.Clear();
                List = null;
            }

            await EnsureIsLoadedAsync(cancelToken);

            return this;
        }

        public void Clear()
        {
            if (List != null)
            {
                List.Clear();
                List = null;
            }
        }
    }
}
