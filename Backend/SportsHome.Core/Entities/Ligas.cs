using System.ComponentModel.DataAnnotations;

namespace SportsHome.Core.Entities
{
    public class Ligas
    {
        [Key]
        public int LigaId { get; set; } // ID de la API
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Logo { get; set; }

        public ICollection<LigasTemporadas> Temporadas { get; set; }
        public ICollection<Partidos> Partidos { get; set; }
        public ICollection<Clasificaciones> Clasificaciones { get; set; }
    }
}
