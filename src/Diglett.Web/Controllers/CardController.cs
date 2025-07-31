using Diglett.Core.Data;
using Diglett.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly DiglettDbContext _db;

        public CardController(DiglettDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _db.Cards
                .AsNoTracking()
                .Take(100)
                .Select(x => new CardModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = $"/img/cards/{x.Set.Serie.Category.ToString().ToLower()}/{x.Set.SerieId}/{x.SetId}/{x.Code}.png"
            }).ToListAsync();

            return View(cards);
        }
    }
}
