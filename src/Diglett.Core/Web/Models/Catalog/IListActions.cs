using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Search;
using Diglett.Core.Collections;

namespace Diglett.Web.Models.Catalog
{
    public interface IListActions
    {
        string? SearchTerm { get; }
        Category? CurrentCategory { get; }

        IPageable PagedList { get; }
        IEnumerable<int> AvailablePageSizes { get; }

        CardSortingEnum? CurrentSortOrder { get; }
    }
}
