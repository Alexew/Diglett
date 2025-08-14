using Diglett.Core.Catalog.Variants;
using Diglett.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Diglett.Core.Catalog.Collection
{
    public class BinderItem : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int Slot { get; set; } = 1;

        public int BinderId { get; set; }
        public Binder Binder { get; set; } = null!;

        public int CardVariantId { get; set; }
        public CardVariant CardVariant { get; set; } = null!;
    }
}
