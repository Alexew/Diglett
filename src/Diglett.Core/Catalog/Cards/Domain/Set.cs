using Diglett.Core.Domain;

namespace Diglett.Core.Catalog.Cards
{
    public class Set : BaseEntity, IDisplayOrder
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Code { get; set; }
        public DateTime? ReleaseOnUtc { get; set; }

        public int DisplayOrder { get; set; }

        public int SerieId { get; set; }
        public Serie Serie { get; set; } = default!;

        public ICollection<Card> Cards { get; set; } = [];
    }
}
