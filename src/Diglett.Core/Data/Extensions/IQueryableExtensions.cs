using Diglett.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Diglett
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> query, bool tracked)
            where T : BaseEntity
        {
            Guard.NotNull(query);

            return tracked ? query.AsTracking() : query.AsNoTracking();
        }

        public static async Task<List<TEntity>> GetManyAsync<TEntity>(this IQueryable<TEntity> query, IEnumerable<int> ids, bool tracked = false)
            where TEntity : BaseEntity
        {
            Guard.NotNull(query);
            Guard.NotNull(ids);

            if (!ids.Any())
                return [];

            var items = await query
                .ApplyTracking(tracked)
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();

            return items.OrderBySequence(ids).ToList();
        }
    }
}
