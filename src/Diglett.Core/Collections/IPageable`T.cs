namespace Diglett.Core.Collections
{
    public interface IPageable<out T> : IPageable, IEnumerable<T>
    {
        IQueryable<T> SourceQuery { get; }
        Task<int> GetTotalCountAsync();
        IQueryable<T> ApplyPaging();
    }
}
