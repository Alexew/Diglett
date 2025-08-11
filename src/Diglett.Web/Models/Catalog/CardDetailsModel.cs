using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Catalog
{
    public class CardDetailsModel : EntityModelBase
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public List<CardVariantModel> Variants { get; set; } = [];

        public class CardVariantModel : EntityModelBase
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public int Quantity { get; set; }
        }
    }
}
