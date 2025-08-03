using Diglett.Core.Search;
using Microsoft.AspNetCore.Http;

namespace Diglett.Core.Catalog.Search.Modelling
{
    public class CatalogSearchQueryFactory : SearchQueryFactoryBase, ICatalogSearchQueryFactory
    {
        public CatalogSearchQueryFactory(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }

        public CatalogSearchQuery? Current { get; private set; }

        protected override string[] Tokens => ["i", "s"];

        public CatalogSearchQuery? CreateFromQuery()
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx?.Request == null)
                return null;

            var query = new CatalogSearchQuery();

            ConvertPaging(query);

            return query;
        }

        protected virtual void ConvertPaging(CatalogSearchQuery query)
        {
            var index = Math.Max(1, GetValueFor<int?>("i") ?? 1);
            var size = GetPageSize(query);

            query.Slice((index - 1) * size, size);
        }

        private int GetPageSize(CatalogSearchQuery query)
        {
            var selectedSize = GetValueFor<int?>("s");
            if (selectedSize.HasValue)
            {
                return selectedSize.Value;
            }

            // TODO: Get size from session

            return 30;
        }
    }
}
