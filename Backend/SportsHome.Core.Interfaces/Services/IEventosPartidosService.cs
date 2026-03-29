using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IEventosPartidosService : IService<EventosPartidos>
    {
        Task<OQuery<EventosPartidos>> GetEventosPartidosAsync(ODataQueryOptions options);
    }
}
