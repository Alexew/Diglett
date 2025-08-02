namespace Diglett.Core.Collections
{
    public interface IPageable
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }

        int PageNumber { get; set; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        int FirstItemIndex { get; }
        int LastItemIndex { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
    }
}
