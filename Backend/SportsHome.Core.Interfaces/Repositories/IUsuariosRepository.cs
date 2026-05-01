using SportsHome.Core.Entities;

namespace SportsHome.Core.Interfaces.Repositories
{
    public interface IUsuariosRepository : IRepository<Usuarios>
    {
        Task<Usuarios?> GetByEmailAsync(string correo);
        Task UpdateAsync(Usuarios entity);
    }
}
