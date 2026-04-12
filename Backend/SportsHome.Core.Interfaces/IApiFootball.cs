using SportsHome.Core.Entities;

namespace SportsHome.Core.Interfaces
{
    public interface IApiFootball
    {
        /// <summary>Número de peticiones realizadas en esta instancia.</summary>
        int RequestCount { get; }

        /// <summary>True cuando se ha alcanzado el límite diario configurado.</summary>
        bool LimitReached { get; }

        Task<(List<Ligas> Ligas, List<LigasTemporadas> Temporadas)> GetLigasConTemporadasAsync();
        Task<List<Equipos>> GetEquiposAsync(int leagueId, int season);
        Task<(List<Jugadores> Jugadores, List<EstadisticasJugadores> Estadisticas)> GetJugadoresConEstadisticasAsync(int teamId, int season);
        Task<List<Partidos>> GetPartidosAsync(int leagueId, int season);
        Task<List<EventosPartidos>> GetEventosAsync(int partidoId);
        Task<List<Clasificaciones>> GetClasificacionAsync(int leagueId, int season);
    }
}
