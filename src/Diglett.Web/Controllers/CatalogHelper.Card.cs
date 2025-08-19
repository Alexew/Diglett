using Diglett.Core.Catalog.Cards;
using Diglett.Web.Models.Catalog;
using Microsoft.EntityFrameworkCore;

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
                ImageUrl = _mediaService.GetImageUrl(card)
            };

            foreach (var variant in card.Variants)
            {
                var vm = new CardDetailsModel.CardVariantModel
                {
                    Id = variant.Id,
                    Name = variant.Name,
                    Description = variant.Description
                };

                if (_workContext.CurrentUser != null)
                {
                    var quantity = await _db.CollectionEntries
                        .AsNoTracking()
                        .CountAsync(x => x.CardVariantId == variant.Id && x.UserId == _workContext.CurrentUser.Id);

                    vm.Quantity = quantity;
                }

                model.Variants.Add(vm);
            }

            return model;
        }
    }
}
