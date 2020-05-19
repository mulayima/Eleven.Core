using Eleven.Core.ApplicationCore.Model.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;

namespace Eleven.Core.ApplicationCore.Persistence.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ConfigureShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var tp in modelBuilder.Model.GetEntityTypes())
            {
                var type = tp.ClrType;

                if (type.IsFullAuditableEntity())
                {
                    modelBuilder.Entity(type).Property<DateTime>("date_create");
                    modelBuilder.Entity(type).Property<DateTime?>("date_modify");
                    modelBuilder.Entity(type).Property<Guid>("uid_user_create");
                    modelBuilder.Entity(type).Property<Guid?>("uid_user_modify");
                }

                if (type.IsCreateOnlyAuditableEntity())
                {
                    modelBuilder.Entity(type).Property<DateTime>("date_create");
                    modelBuilder.Entity(type).Property<Guid>("uid_user_create");
                }

                if (type.IsSoftDeletableEntity())
                {
                    modelBuilder.Entity(type).Property<bool>("is_deleted");
                }
            }

            return modelBuilder;
        }
    }
}
