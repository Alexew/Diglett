using Diglett.Core.Domain;

namespace Diglett.Core.Catalog.Cards
{
    public class Serie : BaseEntity, IDisplayOrder
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Category Category { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<Set> Sets { get; set; } = [];
    }
}
