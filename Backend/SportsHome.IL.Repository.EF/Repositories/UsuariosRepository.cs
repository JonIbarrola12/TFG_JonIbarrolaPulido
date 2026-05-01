using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Repositories;

namespace SportsHome.IL.Repository.EF.Repositories
{
    public class UsuariosRepository : Repository<Usuarios>, IUsuariosRepository
    {
        public UsuariosRepository(SportsHomeContext context) : base(context)
        {
        }

        public SportsHomeContext SportsHomeContext
        {
            get { return Context as SportsHomeContext; }
        }

        public async Task<Usuarios?> GetByEmailAsync(string correo)
        {
            return await SportsHomeContext.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }
        public Task UpdateAsync(Usuarios entity)
        {
            SportsHomeContext.Set<Usuarios>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
