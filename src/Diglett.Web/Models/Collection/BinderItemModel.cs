using Diglett.Web.Modelling;

namespace Diglett.Web.Models.Collection
{
    public class BinderItemModel : EntityModelBase
    {
        public int Slot { get; set; }

        public int CardVariantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
