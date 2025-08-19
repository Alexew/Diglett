using Diglett.Core.Catalog.Cards;
using Diglett.Core.Data;

namespace Diglett.Core.Content.Media
{
    public class MediaService : IMediaService
    {
        private readonly DiglettDbContext _db;

        public MediaService(DiglettDbContext db)
        {
            _db = db;
        }

        // TODO: This should be redo, obviously
        public string GetImageUrl(Card card)
        {
            Guard.NotNull(card);

            _db.Entry(card).Reference(c => c.Set).Load();
            _db.Entry(card.Set).Reference(s => s.Serie).Load();

            return $"/img/cards/{card.Set.Serie.Category.ToString().ToLower()}/{card.Set.SerieId}/{card.SetId}/{card.Code}.png";
        }
    }
}
