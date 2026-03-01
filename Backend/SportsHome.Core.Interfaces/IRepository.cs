using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Queries.OData;
using System.Linq.Expressions;

namespace SportsHome.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetListAsync();
        Task<OQuery<TEntity>> GetFilteredAsync(ODataQueryOptions options);
        Task<OQuery<TEntity>> GetFilteredAsync(ODataQueryOptions options, Expression<Func<TEntity,bool>> preFilter);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
