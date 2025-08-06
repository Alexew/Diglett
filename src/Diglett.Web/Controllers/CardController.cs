using Diglett.Core.Catalog.Search;
using Microsoft.AspNetCore.Mvc;

namespace Diglett.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly CatalogHelper _helper;
        private readonly ICatalogSearchService _searchService;

        public CardController(
            CatalogHelper helper,
            ICatalogSearchService searchService)
        {
            _helper = helper;
            _searchService = searchService;
        }

        public async Task<IActionResult> Index(CatalogSearchQuery query)
        {
            var result = await _searchService.SearchAsync(query);
            var model = await _helper.MapCardSummaryModelAsync(result);

            _helper.MapListActions(model);

            return View(model);
        }
    }
}
