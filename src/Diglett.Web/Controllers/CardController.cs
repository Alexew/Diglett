using Diglett.Core;
using Diglett.Core.Catalog.Collection;
using Diglett.Core.Catalog.Search;
using Diglett.Core.Content.Media;
using Diglett.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web.Controllers
{
    public partial class CardController : DiglettController
    {
        private readonly DiglettDbContext _db;
        private readonly CatalogHelper _helper;
        private readonly ICatalogSearchService _searchService;
        private readonly IMediaService _mediaService;
        private readonly IWorkContext _workContext;

        public CardController(
            DiglettDbContext db,
            CatalogHelper helper,
            ICatalogSearchService searchService,
            IMediaService mediaService,
            IWorkContext workContext)
        {
            _db = db;
            _helper = helper;
            _searchService = searchService;
            _mediaService = mediaService;
            _workContext = workContext;
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

        [Authorize]
        public async Task<IActionResult> IncrementVariant(int cardVariantId)
        {
            var cardVariant = await _db.CardVariants
                .Include(x => x.Card)
                .SingleOrDefaultAsync(x => x.Id == cardVariantId);

            if (cardVariant == null)
                return NotFound();

            var collectionEntry = new CollectionEntry
            {
                UserId = _workContext.CurrentUser!.Id,
                CardVariantId = cardVariantId
            };

            _db.CollectionEntries.Add(collectionEntry);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = cardVariant.Card.Id });
        }

        [Authorize]
        public async Task<IActionResult> DecrementVariant(int cardVariantId)
        {
            var cardVariant = await _db.CardVariants
                .Include(x => x.Card)
                .SingleOrDefaultAsync(x => x.Id == cardVariantId);

            if (cardVariant == null)
                return NotFound();

            var collectionEntry = await _db.CollectionEntries
                .FirstOrDefaultAsync(x => x.CardVariantId == cardVariantId && x.UserId == _workContext.CurrentUser!.Id);

            if (collectionEntry == null)
                return RedirectToAction(nameof(Details), new { id = cardVariant.Card.Id });

            _db.CollectionEntries.Remove(collectionEntry);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = cardVariant.Card.Id });
        }
    }
}
