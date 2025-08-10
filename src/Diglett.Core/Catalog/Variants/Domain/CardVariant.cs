using Diglett.Core.Catalog.Cards;
using Diglett.Core.Domain;

namespace Diglett.Core.Catalog.Variants
{
    public class CardVariant : BaseEntity, IDisplayOrder
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; } = default!;
    }
}
