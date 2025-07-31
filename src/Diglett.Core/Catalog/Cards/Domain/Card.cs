using Diglett.Core.Domain;

namespace Diglett.Core.Catalog.Cards
{
    public class Card : BaseEntity, IDisplayOrder
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public int SetId { get; set; }
        public Set Set { get; set; } = default!;
    }
}
