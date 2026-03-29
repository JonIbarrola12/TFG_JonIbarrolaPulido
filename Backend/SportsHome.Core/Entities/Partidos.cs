using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class Partidos
    {
        public int PartidoId { get; set; }
        public int ExternalId { get; set; }
        public int LigaId { get; set; }
        public int Temporada { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Ronda { get; set; }
        public string Arbitro { get; set; }
        public string ZonaHoraria { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
        public Ligas Liga { get; set; }
        public Equipos EquipoLocal { get; set; }
        public Equipos EquipoVisitante { get; set; }

        public ICollection<EstadisticasEquiposPartidos> Estadisticas { get; set; }
    }
}
