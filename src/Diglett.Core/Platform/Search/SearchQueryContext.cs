namespace Diglett.Core.Search
{
    public class SearchQueryContext<TQuery>
        where TQuery : ISearchQuery
    {
        public SearchQueryContext(TQuery query)
        {
            Guard.NotNull(query);

            SearchQuery = query;
            Filters.AddRange(query.Filters);
        }

        public TQuery SearchQuery { get; }
        public List<ISearchFilter> Filters { get; } = [];
    }
}
