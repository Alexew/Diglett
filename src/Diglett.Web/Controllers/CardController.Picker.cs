using Diglett.Core.Catalog.Search;
using Diglett.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Diglett.Web.Controllers
{
    public partial class CardController
    {
        public IActionResult Picker(CardPickerModel model)
        {
            return PartialView("Partials/Picker", model);
        }

        [HttpPost]
        [ActionName("Picker")]
        public async Task<IActionResult> PickerPost(CardPickerModel model)
        {
            var skip = model.PageIndex * model.PageSize;

            model.SearchTerm = model.SearchTerm.TrimSafe();

            var searchQuery = new CatalogSearchQuery(model.SearchTerm)
                .Slice(skip, model.PageSize)
                .SortBy(CardSortingEnum.CardCodeAsc);

            if (model.Category.HasValue)
            {
                searchQuery = searchQuery.WithCategory(model.Category.Value);
            }

            var searchResult = await _searchService.SearchAsync(searchQuery);
            var hits = await searchResult.GetHitsAsync();
            var cards = new List<CardPickerModel.SearchResultModel>();

            foreach (var hit in hits)
            {
                // TODO: Optimize
                await _db.Entry(hit).Collection(x => x.Variants).LoadAsync();

                var primaryVariant = hit.Variants.FirstOrDefault(x => x.IsPrimary) ??
                    hit.Variants.FirstOrDefault();

                if (primaryVariant == null)
                    continue;

                var card = new CardPickerModel.SearchResultModel
                {
                    Id = primaryVariant.Id,
                    Name = hit.Name,
                    Code = hit.Code,
                    ImageUrl = _mediaService.GetImageUrl(hit)
                };

                cards.Add(card);
            }

            model.SearchResult = cards;

            return PartialView("Partials/Picker.List", model);
        }
    }
}
