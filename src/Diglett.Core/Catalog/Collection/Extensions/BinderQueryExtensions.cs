using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Collection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Diglett
{
    public static class BinderQueryExtensions
    {
        public static IIncludableQueryable<Binder, Card> IncludeCards(this IQueryable<Binder> query)
        {
            Guard.NotNull(query);

            return query.Include(x => x.Items)
                .ThenInclude(x => x.CardVariant)
                .ThenInclude(x => x.Card);
        }

        public static IQueryable<Binder> ApplyUserFilter(this IQueryable<Binder> query, int userId)
        {
            Guard.NotNull(query);

            return query.Where(x => x.UserId == userId);
        }

        public static IIncludableQueryable<BinderItem, Card> IncludeCards(this IQueryable<BinderItem> query)
        {
            Guard.NotNull(query);

            return query.Include(x => x.CardVariant)
                .ThenInclude(x => x.Card);
        }

        public static IQueryable<BinderItem> ApplyBinderFilter(this IQueryable<BinderItem> query, int binderId)
        {
            Guard.NotNull(query);

            return query.Where(x => x.BinderId == binderId);
        }

        public static IQueryable<BinderItem> ApplyPageFilter(this IQueryable<BinderItem> query, int page, int pocketSize)
        {
            Guard.NotNull(query);

            var fromSlot = (page - 1) * pocketSize;
            var toSlot = page * pocketSize;

            return query.ApplySlotRangeFilter(fromSlot, toSlot);
        }

        public static IQueryable<BinderItem> ApplySlotFilter(this IQueryable<BinderItem> query, int slot)
        {
            Guard.NotNull(query);

            return query.Where(x => x.Slot == slot);
        }

        public static IQueryable<BinderItem> ApplySlotRangeFilter(this IQueryable<BinderItem> query, int fromSlot, int toSlot)
        {
            Guard.NotNull(query);

            return query.Where(x => x.Slot >= fromSlot && x.Slot <= toSlot);
        }
    }
}
