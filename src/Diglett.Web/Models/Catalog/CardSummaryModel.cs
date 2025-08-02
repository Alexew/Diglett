using Diglett.Core.Collections;
using Diglett.Core.Utilities;
using Diglett.Core.Web.Modelling;
using Diglett.Core.Web.Models.Catalog;

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

        public IPageable PagedList { get; }

        public void Dispose()
        {
            Items?.Clear();
        }
    }
}
