using Diglett.Core.Catalog.Cards;
using Diglett.Core.Search;
using Microsoft.AspNetCore.Http;

namespace Diglett.Core.Catalog.Search.Modelling
{
    public class CatalogSearchQueryFactory : SearchQueryFactoryBase, ICatalogSearchQueryFactory
    {
        public CatalogSearchQueryFactory(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }

        public CatalogSearchQuery? Current { get; private set; }

        protected override string[] Tokens => ["q", "i", "s", "o", "c"];

        public CatalogSearchQuery? CreateFromQuery()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Request == null)
                return null;

            var term = GetValueFor<string>("q");
            var query = new CatalogSearchQuery(term);

            ConvertPaging(query);
            ConvertSorting(query);
            ConvertCategory(query);

            Current = query;

            return query;
        }

        protected virtual void ConvertPaging(CatalogSearchQuery query)
        {
            var index = Math.Max(1, GetValueFor<int?>("i") ?? 1);
            var size = GetPageSize(query);

            query.Slice((index - 1) * size, size);
        }

        protected virtual void ConvertSorting(CatalogSearchQuery query)
        {
            var sort = GetValueFor<CardSortingEnum?>("o")
                ?? CardSortingEnum.Initial;

            query.CustomData["CurrentSortOrder"] = sort;
            query.SortBy(sort);
        }

        private int GetPageSize(CatalogSearchQuery query)
        {
            var selectedSize = GetValueFor<int?>("s");
            if (selectedSize.HasValue)
            {
                // TODO: Validate size against available options
                return selectedSize.Value;
            }

            // TODO: Get size from session

            return 30;
        }

        protected virtual void ConvertCategory(CatalogSearchQuery query)
        {
            var category = GetValueFor<Category?>("c");
            if (category == null)
                return;

            query.CustomData["CurrentCategory"] = category;
            query.WithCategory(category.Value);
        }
    }
}
