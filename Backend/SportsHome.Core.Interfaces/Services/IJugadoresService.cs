using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IJugadoresService : IService<Jugadores>
    {
        Task<OQuery<Jugadores>> GetJugadoresAsync(ODataQueryOptions options);
    }
}
