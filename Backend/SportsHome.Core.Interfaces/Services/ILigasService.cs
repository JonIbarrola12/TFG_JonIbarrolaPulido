using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface ILigasService : IService<Ligas>
    {
        Task<OQuery<Ligas>> GetLigasAsync(ODataQueryOptions options);
    }
}
