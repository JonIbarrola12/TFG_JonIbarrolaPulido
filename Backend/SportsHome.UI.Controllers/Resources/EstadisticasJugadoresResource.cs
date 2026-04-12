namespace SportsHome.UI.Controllers.Resources
{
    public class EstadisticasJugadoresResource
    {
        public int EstadisticaJugadorId { get; set; }
        public int JugadorId { get; set; }
        public int EquipoId { get; set; }
        public int LigaId { get; set; }
        public int Temporada { get; set; }
        public int? Apariciones { get; set; }
        public int? Goles { get; set; }
        public int? Asistencias { get; set; }
        public int? TarjetasAmarillas { get; set; }
        public int? TarjetasRojas { get; set; }
        public int? Minutos { get; set; }
        public JugadoresResource Jugador { get; set; }
        public EquiposResource Equipo { get; set; }
        public LigasResource Liga { get; set; }
    }
}
