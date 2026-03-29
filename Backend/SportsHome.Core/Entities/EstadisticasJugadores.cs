using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class EstadisticasJugadores
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
        public Jugadores Jugador { get; set; }
        public Equipos Equipo { get; set; }
        public Ligas Liga { get; set; }
    }

}
