using Diglett.Core.Catalog.Collection;
using Microsoft.AspNetCore.Identity;

namespace Diglett.Core.Domain.Identity
{
    public class User : IdentityUser<int>, ITimestamped
    {
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }

        public ICollection<CollectionEntry> CollectionEntries { get; set; } = [];
    }
}