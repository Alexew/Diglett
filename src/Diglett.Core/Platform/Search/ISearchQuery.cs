namespace Diglett.Core.Search
{
    public interface ISearchQuery
    {
        // Paging
        int Skip { get; }
        int Take { get; }
    }
}
