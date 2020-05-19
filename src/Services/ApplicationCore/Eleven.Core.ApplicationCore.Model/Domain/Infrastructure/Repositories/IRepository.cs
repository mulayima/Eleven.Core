using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Entities;

namespace Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Repositories
{
    public interface IRepository<TEntity, TId>
           where TEntity : Entity<TId>
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TId id);

        Task<TEntity> GetByIdAsNoTrackingAsync(TId id);

        Task CreateAsync(TEntity entity);

        void Update(TEntity entity);

        Task DeleteAsync(TId id);

        void Delete(TEntity entity);

        TEntity CreateProxy(TId id);

        T GetShadowPropertyValue<T>(TEntity entity, string propertyName);

        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate);

    }
}
