using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IEquiposService : IService<Equipos>
    {
        Task <OQuery<Equipos>> GetEquiposAsync(ODataQueryOptions options);
    }
}
