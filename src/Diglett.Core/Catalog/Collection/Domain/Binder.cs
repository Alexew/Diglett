using Diglett.Core.Domain;
using Diglett.Core.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Diglett.Core.Catalog.Collection
{
    public class Binder : BaseEntity
    {
        public string Name { get; set; } = null!;

        [Range(1, 50)]
        public int PageCount { get; set; } = 1;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<BinderItem> Items { get; set; } = [];
    }
}
