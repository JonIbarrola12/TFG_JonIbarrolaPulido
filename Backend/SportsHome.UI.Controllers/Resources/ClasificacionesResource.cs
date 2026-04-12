namespace SportsHome.UI.Controllers.Resources
{
    public class ClasificacionesResource
    {
        public int ClasificacionId { get; set; }
        public int LigaId { get; set; }
        public int Temporada { get; set; }
        public int EquipoId { get; set; }
        public int Jugados { get; set; }
        public int Ganados { get; set; }
        public int Empatados { get; set; }
        public int Perdidos { get; set; }
        public int GolesAFavor { get; set; }
        public int GolesEnContra { get; set; }
        public int Puntos { get; set; }
        public int Posicion { get; set; }
        public LigasResource Liga { get; set; }
        public EquiposResource Equipo { get; set; }
    }
}
