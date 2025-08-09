using Diglett.Core.Catalog.Cards;
using Diglett.Core.Search;

namespace Diglett.Core.Catalog.Search
{
    public class CatalogSearchQueryVisitor : LinqSearchQueryVisitor<Card, CatalogSearchQuery, SearchQueryContext<CatalogSearchQuery>>
    {
        protected override IQueryable<Card> VisitFilter(
            ISearchFilter filter,
            SearchQueryContext<CatalogSearchQuery> context,
            IQueryable<Card> query)
        {
            var names = CatalogSearchQuery.KnownFilters;
            var fieldName = filter.FieldName;

            if (fieldName == names.Name)
            {
                return VisitTermFilter((IAttributeSearchFilter)filter, query);
            }
            else if (fieldName == names.CategoryId)
            {
                return VisitCategoryFilter((IAttributeSearchFilter)filter, query);
            }

            return query;
        }

        protected virtual IQueryable<Card> VisitTermFilter(IAttributeSearchFilter filter, IQueryable<Card> query)
        {
            var term = filter.Term?.ToString();
            if (!term.HasValue())
                return query;

            return
                from card in query
                where card.Name.Contains(term!)
                select card;
        }

        protected virtual IQueryable<Card> VisitCategoryFilter(IAttributeSearchFilter filter, IQueryable<Card> query)
        {
            var category = (Category)filter.Term!;

            return
                from card in query
                where card.Set.Serie.Category == category
                select card;
        }

        protected override IQueryable<Card> VisitSorting(
            SearchSort sorting,
            SearchQueryContext<CatalogSearchQuery> context,
            IQueryable<Card> query)
        {
            var names = CatalogSearchQuery.KnownSortings;

            if (sorting.FieldName == names.Name)
            {
                query = OrderBy(query, x => x.Name, sorting.Descending);
            }
            else if (sorting.FieldName == names.CardCode)
            {
                query = OrderBy(query, x => x.Code, sorting.Descending);
            }

            return ApplyDefaultSorting(context, query);
        }

        protected override IQueryable<Card> ApplyDefaultSorting(SearchQueryContext<CatalogSearchQuery> context, IQueryable<Card> query)
        {
            // TODO: Redo with display order field
            query = OrderBy(query, x => x.Set.Serie.Id);
            query = OrderBy(query, x => x.Set.Id);
            query = OrderBy(query, x => x.Id);

            return query;
        }
    }
}
