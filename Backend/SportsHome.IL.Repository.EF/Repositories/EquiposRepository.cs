
using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Repositories;

namespace SportsHome.IL.Repository.EF.Repositories
{
    public class EquiposRepository : Repository<Equipos>, IEquiposRepository
    {
        public EquiposRepository(SportsHomeContext context) : base(context)
    {
    }

    public SportsHomeContext SportsHomeContext
    {
        get { return Context as SportsHomeContext; }
    }
}
}
