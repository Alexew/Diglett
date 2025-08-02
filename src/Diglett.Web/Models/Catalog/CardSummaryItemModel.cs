using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Catalog
{
    public class CardSummaryItemModel : EntityModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
