using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.UI.Controllers.Resources
{
    public class LigasResource
    {
        public int LigaId { get; set; } // ID de la API
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Logo { get; set; }
    }
}
