using System.ComponentModel.DataAnnotations;

namespace SportsHome.Core.Entities
{
    public class Ligas
    {
        [Key]
        public int LigaId { get; set; } // ID de la API
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Logo { get; set; }
        public int Temporada { get; set; }

    }
}
