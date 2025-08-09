using Diglett.Core.Domain;
using System.Linq.Expressions;

namespace Diglett.Core.Search
{
    public abstract class LinqSearchQueryVisitor<TEntity, TQuery, TContext>
        where TEntity : BaseEntity
        where TQuery : ISearchQuery
        where TContext : SearchQueryContext<TQuery>
    {
        public IQueryable<TEntity> Visit(TContext context, IQueryable<TEntity> query)
        {
            Guard.NotNull(context);
            Guard.NotNull(query);

            // Filters
            for (var i = 0; i < context.Filters.Count; i++)
            {
                query = VisitFilter(context.Filters[i], context, query);
            }

            // Sorting
            var sortings = context.SearchQuery.Sorting;
            if (sortings.Count > 0)
            {
                foreach (var sorting in sortings)
                {
                    query = VisitSorting(sorting, context, query);
                }
            }
            else
            {
                query = ApplyDefaultSorting(context, query);
            }

            return query;
        }

        protected abstract IQueryable<TEntity> VisitFilter(ISearchFilter filter, TContext context, IQueryable<TEntity> query);

        protected abstract IQueryable<TEntity> VisitSorting(SearchSort sorting, TContext context, IQueryable<TEntity> query);

        protected virtual IQueryable<TEntity> ApplyDefaultSorting(TContext context, IQueryable<TEntity> query)
        {
            return OrderBy(query, x => x.Id);
        }

        protected IOrderedQueryable<TEntity> OrderBy<TKey>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TKey>> keySelector,
            bool descending = false)
        {
            var isOrdered = query.Expression.Type == typeof(IOrderedQueryable<TEntity>);

            if (isOrdered)
            {
                if (descending)
                    return ((IOrderedQueryable<TEntity>)query).ThenByDescending(keySelector);
                else
                    return ((IOrderedQueryable<TEntity>)query).ThenBy(keySelector);
            }
            else
            {
                if (descending)
                    return query.OrderByDescending(keySelector);
                else
                    return query.OrderBy(keySelector);
            }
        }
    }
}
