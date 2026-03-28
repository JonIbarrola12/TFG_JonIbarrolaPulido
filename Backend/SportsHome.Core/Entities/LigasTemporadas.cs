namespace SportsHome.Core.Entities
{
    public class LigasTemporadas
    {
        public int LigaTemporadaId { get; set; }

        public int LigaId { get; set; }
        public int Temporada { get; set; }

        public Ligas Liga { get; set; }
    }
}
