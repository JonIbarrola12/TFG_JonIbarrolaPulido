using SportsHome.Core.Interfaces.Repositories;

namespace SportsHome.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILigasRepository Ligas { get; }
        IEquiposRepository Equipos { get; }
        IJugadoresRepository Jugadores { get; }
        IPartidosRepository Partidos { get; }   
        IEventosPartidosRepository EventosPartidos { get; }
        IEstadisticasJugadoresRepository EstadisticasJugadores { get; }
        IClasificacionesRepository Clasificaciones { get; }
        ISyncLogsRepository SyncLogs { get; }
        IUsuariosRepository Usuarios { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        Task<int> Complete();
    }
}
