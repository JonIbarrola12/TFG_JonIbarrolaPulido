using SportsHome.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsHome.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILigasRepository Ligas { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        Task<int> Complete();
    }
}
