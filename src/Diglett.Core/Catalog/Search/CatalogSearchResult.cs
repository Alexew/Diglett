using Diglett.Core.Catalog.Cards;
using Diglett.Core.Collections;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Catalog.Search
{
    public class CatalogSearchResult : Disposable
    {
        private IPagedList<Card>? _hits;
        private readonly CatalogSearchQuery _query;
        private readonly DbSet<Card> _dbSet;

        public CatalogSearchResult(
            CatalogSearchQuery query,
            DbSet<Card> dbSet,
            int totalHitsCount,
            int[]? hitsEntityIds)
        {
            Guard.NotNull(query, nameof(query));

            _query = query;
            _dbSet = dbSet;

            TotalHitsCount = totalHitsCount;
            HitsEntityIds = hitsEntityIds ?? [];
        }

        public int[] HitsEntityIds { get; }
        public int TotalHitsCount { get; }

        public async Task<IPagedList<Card>> GetHitsAsync()
        {
            if (_hits == null)
            {
                var cards = TotalHitsCount > 0 ?
                    await _dbSet.GetManyAsync(HitsEntityIds) :
                    Enumerable.Empty<Card>();

                _hits = cards.ToPagedList(_query.PageIndex, _query.Take, TotalHitsCount);
            }

            return _hits;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                _hits?.Clear();
            }
        }
    }
}
