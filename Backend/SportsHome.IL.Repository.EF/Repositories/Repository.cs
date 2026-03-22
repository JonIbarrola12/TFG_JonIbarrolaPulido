using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Queries.OData;
using System.Linq.Expressions;

namespace SportsHome.IL.Repository.EF.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        protected internal virtual IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<TEntity?> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<OQuery<TEntity>> GetFilteredAsync(ODataQueryOptions options)
        {
            IQueryable<TEntity> query = Query();

            OQuery<TEntity> oQuery = new OQuery<TEntity>();

            if (options == null)
            {
                oQuery.TotalRegistros = await query.CountAsync();
                oQuery.Resultado = await query.ToListAsync();
                return oQuery;
            }

            if (options.Filter != null)
            {
                oQuery.TotalRegistros = await query.CountAsync();
                query = options.ApplyTo(query) as IQueryable<TEntity>;
            }
            else 
            {
                query = options.ApplyTo(query, AllowedQueryOptions.Skip | AllowedQueryOptions.Top) as IQueryable<TEntity>;

                oQuery.TotalRegistros = await query.CountAsync();

                query = options.ApplyTo(query, AllowedQueryOptions.Filter) as IQueryable<TEntity>;
            }

            oQuery.Resultado = await query.ToListAsync();
            return oQuery;
        }

        public virtual async Task<OQuery<TEntity>> GetFilteredAsync(ODataQueryOptions options, Expression<Func<TEntity, bool>> preFilter)
        {
            IQueryable<TEntity> query = Query().Where(preFilter);

            OQuery<TEntity> oQuery = new OQuery<TEntity>();

            if (options == null)
            {
                oQuery.TotalRegistros = await query.CountAsync();
                oQuery.Resultado = await query.ToListAsync();
                return oQuery;
            }

            if (options.Filter != null)
            {
                oQuery.TotalRegistros = await query.CountAsync();
                query = options.ApplyTo(query) as IQueryable<TEntity>;
            }
            else
            {
                query = options.ApplyTo(query, AllowedQueryOptions.Skip | AllowedQueryOptions.Top) as IQueryable<TEntity>;

                oQuery.TotalRegistros = await query.CountAsync();

                query = options.ApplyTo(query, AllowedQueryOptions.Filter) as IQueryable<TEntity>;
            }

            oQuery.Resultado = await query.ToListAsync();
            return oQuery;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public Task RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

    }
}
