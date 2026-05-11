using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IEstadisticasJugadoresService : IService<EstadisticasJugadores>
    {
        Task<OQuery<EstadisticasJugadores>> GetEstadisticasJugadoresAsync(ODataQueryOptions options);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10GoleadoresAsync(int ligaId, int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10AsistentesAsync(int ligaId, int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10TarjetasAmarillasAsync(int ligaId, int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10TarjetasRojasAsync(int ligaId, int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10MinutosAsync(int ligaId, int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10MejoresJugadoresAsync(int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10MasProblematicosAsync(int? temporada = null);
        Task<IEnumerable<EstadisticasJugadores>> GetTop10MinutosTotalesAsync(int? temporada = null);
    }
}
