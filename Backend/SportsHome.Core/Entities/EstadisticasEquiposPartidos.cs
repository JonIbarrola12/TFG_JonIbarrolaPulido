using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class EstadisticasEquiposPartidos
    {
        public int EstadisticaPartidoEquipoId { get; set; }
        public int PartidoId { get; set; }
        public int EquipoId { get; set; }
        public int? TirosAPuerta { get; set; }
        public int? TirosFuera { get; set; }
        public decimal? Posesion { get; set; }
        public int? Faltas { get; set; }
        public int? Corners { get; set; }
        public int? TarjetasAmarillas { get; set; }
        public int? TarjetasRojas { get; set; }
        public Partidos Partido { get; set; }
        public Equipos Equipo { get; set; }
    }
}
