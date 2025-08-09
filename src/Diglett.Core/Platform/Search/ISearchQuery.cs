namespace Diglett.Core.Search
{
    public interface ISearchQuery
    {
        // Filtering
        ICollection<ISearchFilter> Filters { get; }

        // Paging
        int Skip { get; }
        int Take { get; }

        // Sorting
        ICollection<SearchSort> Sorting { get; }

        // Result
        IDictionary<string, object> CustomData { get; }
    }
}
