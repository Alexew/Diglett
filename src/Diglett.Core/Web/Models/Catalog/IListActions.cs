using Diglett.Core.Catalog.Cards;
using Diglett.Core.Collections;

namespace Diglett.Web.Models.Catalog
{
    public interface IListActions
    {
        Category? CurrentCategory { get; }

        IPageable PagedList { get; }
        IEnumerable<int> AvailablePageSizes { get; }
    }
}
