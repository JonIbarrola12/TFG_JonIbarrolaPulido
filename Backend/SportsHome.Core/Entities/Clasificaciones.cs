using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class Clasificaciones
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
        public Ligas Liga { get; set; }
        public Equipos Equipo { get; set; }
    }

}
