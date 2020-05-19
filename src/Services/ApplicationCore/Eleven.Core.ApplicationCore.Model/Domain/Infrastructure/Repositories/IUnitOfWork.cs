using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eleven.Core.ApplicationCore.Model.Domain.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
        void CommitTransaction();
        void RollbackTransaction();
    }
}
