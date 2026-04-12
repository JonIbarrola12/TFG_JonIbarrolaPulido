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

            #region LigasTemporadas
            CreateMap<LigasTemporadas, LigasTemporadasResource>();
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

            #region Partidos
            CreateMap<Partidos, PartidosResource>();

            CreateMap<PartidosResource, Partidos>()
                .ForMember(p => p.PartidoId, opt => opt.Ignore());

            CreateMap<Partidos, Partidos>()
                .ForMember(p => p.PartidoId, opt => opt.Ignore());
            #endregion

            #region EventosPartidos
            CreateMap<EventosPartidos, EventosPartidosResource>();

            CreateMap<EventosPartidosResource, EventosPartidos>()
                .ForMember(e => e.EventoPartidoId, opt => opt.Ignore());

            CreateMap<EventosPartidos, EventosPartidos>()
                .ForMember(e => e.EventoPartidoId, opt => opt.Ignore());
            #endregion

            #region EstadisticasJugadores
            CreateMap<EstadisticasJugadores, EstadisticasJugadoresResource>();

            CreateMap<EstadisticasJugadoresResource, EstadisticasJugadores>()
                .ForMember(e => e.EstadisticaJugadorId, opt => opt.Ignore());

            CreateMap<EstadisticasJugadores, EstadisticasJugadores>()
                .ForMember(e => e.EstadisticaJugadorId, opt => opt.Ignore());
            #endregion

            #region Clasificaciones
            CreateMap<Clasificaciones, ClasificacionesResource>();

            CreateMap<ClasificacionesResource, Clasificaciones>()
                .ForMember(c => c.ClasificacionId, opt => opt.Ignore());

            CreateMap<Clasificaciones, Clasificaciones>()
                .ForMember(c => c.ClasificacionId, opt => opt.Ignore());
            #endregion

            #region SyncLogs
            CreateMap<SyncLogs, SyncLogsResource>();

            CreateMap<SyncLogsResource, SyncLogs>()
                .ForMember(c => c.SyncLogId, opt => opt.Ignore());

            CreateMap<SyncLogs, SyncLogs>()
                .ForMember(c => c.SyncLogId, opt => opt.Ignore());
            #endregion
        }
    }
}
