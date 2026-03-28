namespace SportsHome.Core.Entities
{
    public class JugadoresEquipos
    {
        public int JugadorEquipoId { get; set; }
        public int JugadorId { get; set; }
        public int EquipoId { get; set; }
        public int Temporada { get; set; }

        public Jugadores Jugador { get; set; }
        public Equipos Equipo { get; set; }
    }
}
