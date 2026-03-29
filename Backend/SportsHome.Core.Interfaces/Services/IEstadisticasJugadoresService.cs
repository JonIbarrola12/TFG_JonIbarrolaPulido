using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IEstadisticasJugadoresService : IService<EstadisticasJugadores>
    {
        Task<OQuery<EstadisticasJugadores>> GetEstadisticasJugadoresAsync(ODataQueryOptions options);
    }
}
