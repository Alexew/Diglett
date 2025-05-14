using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Data
{
    public class DiglettDbContext : DbContext
    {
        public DiglettDbContext(DbContextOptions<DiglettDbContext> options)
            : base(options) { }
    }
}