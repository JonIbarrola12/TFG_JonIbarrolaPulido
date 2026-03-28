using SportsHome.Core.Interfaces.Repositories;

namespace SportsHome.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILigasRepository Ligas { get; }
        IEquiposRepository Equipos { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        Task<int> Complete();
    }
}
