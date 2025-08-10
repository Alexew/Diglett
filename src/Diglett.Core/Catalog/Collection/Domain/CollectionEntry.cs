using Diglett.Core.Catalog.Variants;
using Diglett.Core.Domain;
using Diglett.Core.Domain.Identity;

namespace Diglett.Core.Catalog.Collection
{
    public class CollectionEntry : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CardVariantId { get; set; }
        public CardVariant CardVariant { get; set; } = null!;
    }
}
