using Diglett.Core.Catalog.Cards;
using Diglett.Web.Models.Catalog;

namespace Diglett.Web.Controllers
{
    public partial class CatalogHelper
    {
        public async Task<CardDetailsModel> MapCardDetailsModelAsync(Card card)
        {
            Guard.NotNull(card);

            var model = new CardDetailsModel
            {
                Id = card.Id,
                Name = card.Name,
                Code = card.Code,
                ImageUrl = $"/img/cards/{card.Set.Serie.Category.ToString().ToLower()}/{card.Set.SerieId}/{card.SetId}/{card.Code}.png"
            };

            foreach (var variant in card.Variants)
            {
                model.Variants.Add(new CardDetailsModel.CardVariantModel
                {
                    Id = variant.Id,
                    Name = variant.Name,
                    Description = variant.Description
                });
            }

            return model;
        }
    }
}
