using Diglett.Core.Catalog.Search;
using Diglett.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly DiglettDbContext _db;
        private readonly CatalogHelper _helper;
        private readonly ICatalogSearchService _searchService;

        public CardController(
            DiglettDbContext db,
            CatalogHelper helper,
            ICatalogSearchService searchService)
        {
            _db = db;
            _helper = helper;
            _searchService = searchService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List(CatalogSearchQuery query)
        {
            var result = await _searchService.SearchAsync(query);
            var model = await _helper.MapCardSummaryModelAsync(result);

            _helper.MapListActions(model);

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var card = await _db.Cards
                .AsNoTracking()
                .Include(x => x.Set)
                .ThenInclude(x => x.Serie)
                .Include(x => x.Variants)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (card == null)
                return NotFound();

            var model = await _helper.MapCardDetailsModelAsync(card);
            return View(model);
        }
    }
}
