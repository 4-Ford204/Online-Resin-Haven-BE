using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constants.Enums;
using OPS.Domain.Entities;

namespace OPS.Infrastructure.MSSQL.Interceptors
{
    [Service(ServiceLifetime.Scoped)]
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext == null)
            {
                return;
            }

            foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
            {
                var dateTime = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Status = Status.Active;
                    entry.Entity.CreatedAt = dateTime;
                    entry.Entity.UpdatedAt = dateTime;
                }
                else if (
                    entry.State == EntityState.Modified ||
                    entry.References.Any(r =>
                        r.TargetEntry != null &&
                        r.TargetEntry.Metadata.IsOwned() &&
                        r.TargetEntry.State is EntityState.Added or EntityState.Modified)
                    )
                {
                    entry.Entity.UpdatedAt = dateTime;
                }
            }
        }
    }
}
