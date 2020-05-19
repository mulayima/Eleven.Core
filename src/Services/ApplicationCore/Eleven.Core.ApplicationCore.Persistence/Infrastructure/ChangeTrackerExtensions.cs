using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Eleven.Core.ApplicationCore.Model.Domain.SeedWork;

namespace Eleven.Core.ApplicationCore.Persistence.Infrastructure
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();
            var dateTimeOfChange = DateTime.UtcNow;

            foreach (var entry in changeTracker.Entries())
            {
                ApplyValuesForAuditableEntity(entry, dateTimeOfChange);
                ApplyValuesForTenancyEntity(entry);
                ApplyValuesForSoftDeletableEntity(entry);
            }
        }

        private static void ApplyValuesForTenancyEntity(EntityEntry entry)
        {
            var type = entry.Entity.GetType();

            if (type.IsTenancyEntity() && entry.State == EntityState.Added)
            {
                entry.Property("TenantId").CurrentValue = Guid.NewGuid(); // The value will provided from user session manager.
            }
        }

        private static void ApplyValuesForSoftDeletableEntity(EntityEntry entry)
        {
            var type = entry.Entity.GetType();

            if (type.IsSoftDeletableEntity())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("is_deleted").CurrentValue = true;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property("is_deleted").CurrentValue = false;
                }
            }
        }

        private static void ApplyValuesForAuditableEntity(EntityEntry entry, DateTime utcNowDate)
        {
            var type = entry.Entity.GetType();

            if ((type.IsFullAuditableEntity() || type.IsCreateOnlyAuditableEntity()) && entry.State == EntityState.Added)
            {
                entry.Property("date_create").CurrentValue = utcNowDate;
                entry.Property("uid_user_create").CurrentValue = Guid.NewGuid(); // The value will provided from user session manager.
            }

            if (type.IsFullAuditableEntity() && entry.State == EntityState.Modified)
            {
                entry.Property("date_modify").CurrentValue = utcNowDate;
                entry.Property("uid_user_modify").CurrentValue = Guid.NewGuid(); // The value will provided from user session manager.
            }
        }
    }
}
