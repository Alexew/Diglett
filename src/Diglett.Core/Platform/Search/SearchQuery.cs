namespace Diglett.Core.Search
{
    public class SearchQuery<TQuery> : ISearchQuery
        where TQuery : class, ISearchQuery
    {
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

        public TQuery Slice(int skip, int take)
        {
            Guard.NotNegative(skip);
            Guard.NotNegative(take);

            Skip = skip;
            Take = take;

            return (this as TQuery)!;
        }
    }
}
