using Diglett.Core.Catalog.Cards;
using Diglett.Core.Collections;
using Diglett.Core.Utilities;
using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Catalog
{
    public class CardSummaryModel : ModelBase, IListActions, IDisposable
    {
        public CardSummaryModel(IPageable cards)
        {
            Guard.NotNull(cards, nameof(cards));

            Id = CommonHelper.GenerateRandomInteger();
            PagedList = cards;
        }

        public int Id { get; }
        public List<CardSummaryItemModel> Items { get; set; } = [];

        public string? SearchTerm { get; set; }
        public Category? CurrentCategory { get; set; }

        public IPageable PagedList { get; }
        public IEnumerable<int> AvailablePageSizes { get; set; } = [];

        public void Dispose()
        {
            Items?.Clear();
        }
    }
}
