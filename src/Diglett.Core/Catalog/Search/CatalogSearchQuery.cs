using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diglett.Core.Catalog.Search
{
    [ValidateNever, ModelBinder(typeof(CatalogSearchQueryModelBinder))]
    public class CatalogSearchQuery : SearchQuery<CatalogSearchQuery>
    {
    }
}
