using Diglett.Core.Catalog.Cards;
using Diglett.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Catalog.Search
{
    public class CatalogSearchService : ICatalogSearchService
    {
        private readonly DiglettDbContext _db;

        public CatalogSearchService(DiglettDbContext db)
        {
            _db = db;
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
            var query = _db.Cards;

            return query;
        }
    }
}
