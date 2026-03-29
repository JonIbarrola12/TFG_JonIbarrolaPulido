using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Entities
{
    public class SyncLogs
    {
        public int SyncLogId { get; set; }
        public string Entidad { get; set; }
        public DateTime UltimaSincronizacion { get; set; }
    }

}
