using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Search;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Collections;
using Diglett.Core.Data;
using Diglett.Web.Models.Catalog;

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

        public void MapListActions(CardSummaryModel model)
        {
            var query = _catalogSearchQueryFactory.Current;            

            model.SearchTerm = query?.Term;
            model.CurrentCategory = query?.CustomData.Get("CurrentCategory").Convert<Category?>();
            model.AvailablePageSizes = [30, 60, 120];
            model.CurrentSortOrder = query?.CustomData.Get("CurrentSortOrder").Convert<CardSortingEnum?>();
        }

        public async Task<CardSummaryModel> MapCardSummaryModelAsync(CatalogSearchResult sourceResult)
        {
            return await MapCardSummaryModelAsync(await sourceResult.GetHitsAsync(), sourceResult);
        }

        public async Task<CardSummaryModel> MapCardSummaryModelAsync(IPagedList<Card> cards, CatalogSearchResult sourceResult)
        {
            Guard.NotNull(cards);

            var model = new CardSummaryModel(cards);

            foreach (var card in cards)
            {
                await MapCardSummaryItem(card, model);
            }

            return model;
        }

        private async Task MapCardSummaryItem(Card card, CardSummaryModel model)
        {
            // TODO: Optimization
            await _db.Entry(card).Reference(c => c.Set).LoadAsync();
            await _db.Entry(card.Set).Reference(s => s.Serie).LoadAsync();

            var item = new CardSummaryItemModel
            {
                Id = card.Id,
                Name = card.Name,
                Code = card.Code,
                ImageUrl = $"/img/cards/{card.Set.Serie.Category.ToString().ToLower()}/{card.Set.SerieId}/{card.SetId}/{card.Code}.png"
            };

            model.Items.Add(item);
        }
    }
}
