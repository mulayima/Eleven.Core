using System;

namespace Eleven.Core.ApplicationCore.Model.Domain.SeedWork
{
    public static class EntityInterfaceExtensions
    {
        public static bool IsFullAuditableEntity(this Type entitiyType)
        {
            return typeof(IFullAuditableEntity).IsAssignableFrom(entitiyType);
        }

        public static bool IsCreateOnlyAuditableEntity(this Type entitiyType)
        {
            return typeof(ICreateOnlyAuditableEntity).IsAssignableFrom(entitiyType);
        }

        public static bool IsSoftDeletableEntity(this Type entitiyType)
        {
            return typeof(ISoftDeletableEntity).IsAssignableFrom(entitiyType);
        }

        public static bool IsTenancyEntity(this Type entitiyType)
        {
            return typeof(ITenancyEntity).IsAssignableFrom(entitiyType);
        }
    }
}
