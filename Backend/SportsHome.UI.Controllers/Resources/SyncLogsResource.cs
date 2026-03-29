using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.UI.Controllers.Resources
{
    public class SyncLogsResource
    {
        public int SyncLogId { get; set; }
        public string Entidad { get; set; }
        public DateTime UltimaSincronizacion { get; set; }
    }
}
