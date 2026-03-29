using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.IL.Repository.EF.Repositories
{
    public class EventosPartidosRepository : Repository<EventosPartidos>, IEventosPartidosRepository
    {
        public EventosPartidosRepository(SportsHomeContext context) : base(context)
        {
        }

        public SportsHomeContext SportsHomeContext
        {
            get { return Context as SportsHomeContext; }
        }
    }
}
