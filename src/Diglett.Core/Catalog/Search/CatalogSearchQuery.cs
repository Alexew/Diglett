using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diglett.Core.Catalog.Search
{
    public sealed class KnownFilterNames
    {
        public readonly string CategoryId = "categoryid";
    }

    [ValidateNever, ModelBinder(typeof(CatalogSearchQueryModelBinder))]
    public class CatalogSearchQuery : SearchQuery<CatalogSearchQuery>
    {
        public static readonly KnownFilterNames KnownFilters = new();

        public CatalogSearchQuery WithCategory(Category category)
        {
            return WithFilter(SearchFilter.ByField(KnownFilters.CategoryId, (int)category));
        }
    }
}
