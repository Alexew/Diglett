using Diglett.Core.Catalog.Cards;
using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Catalog
{
    public class CardPickerModel : ModelBase
    {
        public string? SearchTerm { get; set; }
        public Category? Category { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 30;

        public List<SearchResultModel> SearchResult { get; set; } = [];

        public class SearchResultModel : EntityModelBase
        {
            public string? Name { get; set; }
            public string? Code { get; set; }
            public string? ImageUrl { get; set; }
        }
    }
}
