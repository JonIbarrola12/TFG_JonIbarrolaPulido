using AutoMapper;
using SportsHome.Core.Entities;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers.Helpers
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            #region Ligas
            CreateMap<Ligas, LigasResource>();

            CreateMap<LigasResource, Ligas>()
                .ForMember(l => l.LigaId, opt => opt.Ignore());

            CreateMap<Ligas, Ligas>()
                .ForMember(l => l.LigaId, opt => opt.Ignore());
            #endregion

            #region Equipos
            CreateMap<Equipos, EquiposResource>();

            CreateMap<EquiposResource, Equipos>()
                .ForMember(e => e.EquipoId, opt => opt.Ignore());

            CreateMap<Equipos, Equipos>()
                .ForMember(e => e.EquipoId, opt => opt.Ignore());
            #endregion

            #region Jugadores
            CreateMap<Jugadores, JugadoresResource>();

            CreateMap<JugadoresResource, Jugadores>()
                .ForMember(j => j.JugadorId, opt => opt.Ignore());

            CreateMap<Jugadores, Jugadores>()
                .ForMember(j => j.JugadorId, opt => opt.Ignore());
            #endregion
        }
    }
}
