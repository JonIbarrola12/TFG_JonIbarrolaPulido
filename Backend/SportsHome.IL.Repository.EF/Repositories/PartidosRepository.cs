using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Repositories;

namespace SportsHome.IL.Repository.EF.Repositories
{
    public class PartidosRepository : Repository<Partidos>, IPartidosRepository
    {
        public PartidosRepository(SportsHomeContext context) : base(context)
        {
        }

        public SportsHomeContext SportsHomeContext
        {
            get { return Context as SportsHomeContext; }
        }
    }
}
