namespace SportsHome.Core.Interfaces
{
    public interface IService<TEntity> where TEntity : class
    {
        Task <IEnumerable<TEntity>> GetListAsync();
        Task <TEntity?> GetAsync(int id);
        Task <TEntity?> AddAsync (TEntity entity);
        Task Update (TEntity entityOld, TEntity entityNew);
        Task DeleteAsync(TEntity entity);
    }
}
