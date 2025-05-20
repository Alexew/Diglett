using Diglett.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Diglett.Core.Data
{
    public class TimestampedInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateTimestamps(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateTimestamps(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateTimestamps(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<ITimestamped>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedOnUtc = now;

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Entity.UpdatedOnUtc = now;
            }
        }
    }
}