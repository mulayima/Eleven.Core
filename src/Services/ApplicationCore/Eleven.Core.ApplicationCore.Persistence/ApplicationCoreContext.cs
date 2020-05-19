using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eleven.Core.ApplicationCore.Model.Domain.SeedWork;
using Eleven.Core.ApplicationCore.Persistence.Infrastructure;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Repositories;

namespace Eleven.Core.ApplicationCore.Persistence
{
    public class ApplicationCoreContext : DbContext, IUnitOfWork
    {
        public ApplicationCoreContext(DbContextOptions options) : base(options)
        {

        }

        //public DbSet<ElevenUser> Users { get; set; }

        //public DbSet<ElevenProfile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                       .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ConfigureShadowProperties();
            //    .ApplyConfiguration(ElevenUserMapping())
            //    .ApplyConfiguration(ElevenProfileMapping());

            ApplyQueryFilter(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.SetShadowProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await Database.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }

        public void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var tp in modelBuilder.Model.GetEntityTypes())
            {
                var type = tp.ClrType;

                if (type.IsTenancyEntity())
                {
                    if (type.IsSoftDeletableEntity())
                    {
                        var method = ApplyQueryFilterForSoftDeletableAndTenancyEntityMethodInfo.MakeGenericMethod(type);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                    else
                    {
                        var method = ApplyQueryFilterForTenancyEntityMethodInfo.MakeGenericMethod(type);
                        method.Invoke(this, new object[] { modelBuilder });
                    }
                }
                else if (type.IsSoftDeletableEntity())
                {
                    var method = ApplyQueryFilterForSoftDeletableEntityMethodInfo.MakeGenericMethod(type);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        private static readonly MethodInfo ApplyQueryFilterForTenancyEntityMethodInfo
            = typeof(ApplicationCoreContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(ApplicationCoreContext.ApplyQueryFilterForTenancyEntity));

        private static readonly MethodInfo ApplyQueryFilterForSoftDeletableEntityMethodInfo
            = typeof(ApplicationCoreContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(ApplicationCoreContext.ApplyQueryFilterForSoftDeletableEntity));

        private static readonly MethodInfo ApplyQueryFilterForSoftDeletableAndTenancyEntityMethodInfo
          = typeof(ApplicationCoreContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
          .Single(t => t.IsGenericMethod && t.Name == nameof(ApplicationCoreContext.ApplyQueryFilterForSoftDeletableAndTenancyEntity));

        public void ApplyQueryFilterForTenancyEntity<T>(ModelBuilder builder) where T : class
        {
            builder.Entity<T>().HasQueryFilter(item => EF.Property<Guid>(item, "TenantId") == Guid.NewGuid() /*The value will provided from user session manager*/);
        }

        public void ApplyQueryFilterForSoftDeletableEntity<T>(ModelBuilder builder) where T : class
        {
            builder.Entity<T>().HasQueryFilter(item => !EF.Property<bool>(item, "is_deleted"));
        }

        public void ApplyQueryFilterForSoftDeletableAndTenancyEntity<T>(ModelBuilder builder) where T : class
        {
            builder.Entity<T>().HasQueryFilter(item => !EF.Property<bool>(item, "is_deleted")
                    && EF.Property<Guid>(item, "TenantId") == Guid.NewGuid() /*The value will provided from user session manager*/);
        }
    }
}
