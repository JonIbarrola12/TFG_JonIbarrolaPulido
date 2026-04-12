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
    }
}
