using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface ISyncLogsService : IService<SyncLogs>
    {
        Task SyncFullAsync();
        Task SyncLigasYTemporadasAsync();
        Task<List<int>> SyncEquiposAsync(int ligaExternalId, int temporada);
        Task SyncJugadoresYEstadisticasAsync(int equipoExternalId, int temporada);
        Task SyncPartidosAsync(int ligaExternalId, int temporada);
        Task SyncEventosAsync(int partidoExternalId);
        Task SyncClasificacionAsync(int ligaExternalId, int temporada);

        Task<OQuery<SyncLogs>> GetSyncLogsAsync(ODataQueryOptions options);
    }
}
