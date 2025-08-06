using Diglett.Core.Catalog.Cards;
using Diglett.Core.Data;
using Diglett.Core.Search;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Catalog.Search
{
    public class CatalogSearchService : ICatalogSearchService
    {
        private readonly DiglettDbContext _db;
        private readonly CatalogSearchQueryVisitor _queryVisitor;

        public CatalogSearchService(
            DiglettDbContext db,
            CatalogSearchQueryVisitor queryVisitor)
        {
            _db = db;
            _queryVisitor = queryVisitor;
        }

        public async Task<CatalogSearchResult> SearchAsync(CatalogSearchQuery searchQuery)
        {
            var totalHits = 0;
            int[]? hitsEntityIds = null;

            if (searchQuery.Take > 0)
            {
                var query = GetCardQuery(searchQuery);

                totalHits = await query.CountAsync();

                var skip = searchQuery.PageIndex * searchQuery.Take;

                query = query
                    .Skip(skip)
                    .Take(searchQuery.Take);

                hitsEntityIds = query.Select(x => x.Id).ToArray();
            }

            var result = new CatalogSearchResult(
                searchQuery,
                _db.Cards,
                totalHits,
                hitsEntityIds);

            return result;
        }

        private IQueryable<Card> GetCardQuery(CatalogSearchQuery searchQuery)
        {
            var context = new SearchQueryContext<CatalogSearchQuery>(searchQuery);
            var query = _db.Cards.AsQueryable();

            query = _queryVisitor.Visit(context, query);

            return query;
        }
    }
}
