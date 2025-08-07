namespace Diglett.Core.Search
{
    public class SearchQuery<TQuery> : ISearchQuery
        where TQuery : class, ISearchQuery
    {
        private Dictionary<string, object>? _customData;

        public SearchQuery(string? term = null)
        {
            Term = term;

            if (term.HasValue())
            {
                WithFilter(SearchFilter.ByField("name", term!));
            }
        }

        public string? Term { get; }

        // Filtering
        public ICollection<ISearchFilter> Filters { get; } = [];

        // Paging
        public int Skip { get; protected set; }
        public int Take { get; protected set; } = int.MaxValue;
        public int PageIndex
        {
            get
            {
                if (Take == 0)
                    return 0;

                return Math.Max(Skip / Take, 0);
            }
        }

        // Result
        public IDictionary<string, object> CustomData =>
            _customData ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        #region Fluent builder

        public TQuery Slice(int skip, int take)
        {
            Guard.NotNegative(skip);
            Guard.NotNegative(take);

            Skip = skip;
            Take = take;

            return (this as TQuery)!;
        }

        public TQuery WithFilter(ISearchFilter filter)
        {
            Guard.NotNull(filter);

            Filters.Add(filter);

            return (this as TQuery)!;
        }

        #endregion
    }
}
