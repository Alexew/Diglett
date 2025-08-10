using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Data;

namespace Diglett.Web.Controllers
{
    public partial class CatalogHelper
    {
        private readonly DiglettDbContext _db;
        private readonly ICatalogSearchQueryFactory _catalogSearchQueryFactory;

        public CatalogHelper(DiglettDbContext db, ICatalogSearchQueryFactory catalogSearchQueryFactory)
        {
            _db = db;
            _catalogSearchQueryFactory = catalogSearchQueryFactory;
        }

        public string GetImageUrl(Card card)
        {
            Guard.NotNull(card);
            Guard.NotNull(card.Set);
            Guard.NotNull(card.Set.Serie);

            return $"/img/cards/{card.Set.Serie.Category.ToString().ToLower()}/{card.Set.SerieId}/{card.SetId}/{card.Code}.png";
        }
    }
}
