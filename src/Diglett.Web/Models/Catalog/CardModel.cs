using Diglett.Core.Web.Modelling;

namespace Diglett.Web.Models.Catalog
{
    public class CardModel : EntityModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
