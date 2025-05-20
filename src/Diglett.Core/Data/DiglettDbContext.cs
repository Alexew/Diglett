using Diglett.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Core.Data
{
    public class DiglettDbContext : IdentityDbContext<User, IdentityRole<int>, int>
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