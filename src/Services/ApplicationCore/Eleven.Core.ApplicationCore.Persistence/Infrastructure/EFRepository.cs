using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Entities;
using Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Repositories;

namespace Eleven.Core.ApplicationCore.Persistence.Infrastructure
{
    public class EFRepository<TEntity, TId> : IRepository<TEntity, TId>
            where TEntity : Entity<TId>
    {
        private readonly ApplicationCoreContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public EFRepository(ApplicationCoreContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public TEntity CreateProxy(TId id)
        {
            return _context.Set<TEntity>().CreateProxy(id);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsNoTrackingAsync(TId id)
        {
            return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public T GetShadowPropertyValue<T>(TEntity entity, string propertyName)
        {
            return (T)_context.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().CountAsync();
        }
    }
}
