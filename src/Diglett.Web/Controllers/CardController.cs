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

        public async Task<IActionResult> Index()
        {
            var searchQuery = new CatalogSearchQuery()
                .Slice(0, 30);

            var result = await _searchService.SearchAsync(searchQuery);
            var model = await _helper.MapCardSummaryModelAsync(result);

            return View(model);
        }
    }
}
