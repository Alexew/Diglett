using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Data
{
    public class DiglettDbContext : DbContext
    {
        private readonly TimestampedInterceptor _timestampedInterceptor;

        public DiglettDbContext(
            DbContextOptions<DiglettDbContext> options,
            TimestampedInterceptor timestampInterceptor)
            : base(options)
        {
            _timestampedInterceptor = timestampInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.AddInterceptors(_timestampedInterceptor);
        }
    }
}