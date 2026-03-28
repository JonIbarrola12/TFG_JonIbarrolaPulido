using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class Equipos
    {
        public int EquipoId { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public int? Fundacion { get; set; }
        public string Logo { get; set; }
        public string NombreEstadio { get; set; }
        public int? CapacidadEstadio { get; set; }
    }
}
