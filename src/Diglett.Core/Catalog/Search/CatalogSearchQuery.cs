using Diglett.Core.Catalog.Cards;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diglett.Core.Catalog.Search
{
    public sealed class KnownSortingNames
    {
        public readonly string Name = "name";
        public readonly string CardCode = "cardcode";
    }

    public sealed class KnownFilterNames
    {
        public readonly string Name = "name";
        public readonly string CategoryId = "categoryid";
    }

    [ValidateNever, ModelBinder(typeof(CatalogSearchQueryModelBinder))]
    public class CatalogSearchQuery : SearchQuery<CatalogSearchQuery>
    {
        public static readonly KnownSortingNames KnownSortings = new();
        public static readonly KnownFilterNames KnownFilters = new();

        public CatalogSearchQuery(string? term = null)
            : base(term) { }

        public CatalogSearchQuery SortBy(CardSortingEnum sort)
        {
            switch (sort)
            {
                case CardSortingEnum.NameAsc:
                case CardSortingEnum.NameDesc:
                    return SortBy(SearchSort.ByDateTimeField(KnownSortings.Name, sort == CardSortingEnum.NameDesc));

                case CardSortingEnum.CardCodeAsc:
                case CardSortingEnum.CardCodeDesc:
                    return SortBy(SearchSort.ByStringField(KnownSortings.CardCode, sort == CardSortingEnum.CardCodeDesc));

                case CardSortingEnum.Initial:
                default:
                    return this;
            }
        }

        public CatalogSearchQuery WithCategory(Category category)
        {
            return WithFilter(SearchFilter.ByField(KnownFilters.CategoryId, (int)category));
        }
    }

    public enum CardSortingEnum
    {
        Initial = 0,
        NameAsc = 5,
        NameDesc = 6,
        CardCodeAsc = 10,
        CardCodeDesc = 11
    }
}
