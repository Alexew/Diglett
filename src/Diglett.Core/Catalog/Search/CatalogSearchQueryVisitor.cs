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
    }
}
