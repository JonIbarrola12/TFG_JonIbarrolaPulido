using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IPartidosService : IService<Partidos>
    {
        Task<OQuery<Partidos>> GetPartidosAsync(ODataQueryOptions options);
    }
}
