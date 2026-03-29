using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class EventosPartidos
    {
        public int EventoPartidoId { get; set; }
        public int PartidoId { get; set; }
        public int EquipoId { get; set; }
        public int? JugadorId { get; set; }
        public int Minuto { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }
        public string Comentario { get; set; }
        public Partidos Partido { get; set; }
        public Equipos Equipo { get; set; }
        public Jugadores Jugador { get; set; }
    }

}
