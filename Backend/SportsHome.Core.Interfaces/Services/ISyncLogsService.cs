using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Interfaces.Services
{
    public interface ISyncLogsService : IService<SyncLogs>
    {
        Task<OQuery<SyncLogs>> GetSyncLogsAsync(ODataQueryOptions options);
    }
}
