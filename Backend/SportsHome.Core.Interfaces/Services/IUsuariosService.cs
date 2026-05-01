using SportsHome.Core.Entities;

namespace SportsHome.Core.Interfaces.Services
{
    public interface IUsuariosService : IService<Usuarios>
    {
        Task<Usuarios?> GetByEmailAsync(string correo);
        Task UpdateAsync(Usuarios usuario);
    }
}
