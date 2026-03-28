using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.UI.Controllers.Resources
{
    public class JugadoresResource
    {
        public int JugadorId { get; set; }
        public string Nombre { get; set; }
        public string NombrePropio { get; set; }
        public string Apellido { get; set; }
        public int? Edad { get; set; }
        public string Nacionalidad { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }
        public string Foto { get; set; }
    }
}
