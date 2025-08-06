using Diglett.Core.Domain;

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

            for (var i = 0; i < context.Filters.Count; i++)
            {
                query = VisitFilter(context.Filters[i], context, query);
            }

            return query;
        }

        protected abstract IQueryable<TEntity> VisitFilter(ISearchFilter filter, TContext context, IQueryable<TEntity> query);
    }
}
