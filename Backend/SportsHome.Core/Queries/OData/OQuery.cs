using System;
using System.Collections.Generic;
using System.Text;

namespace SportsHome.Core.Queries.OData
{
    public class OQuery<IEntity>
    {
        public IEnumerable<IEntity> Resultado { get; set; }
        public int TotalRegistros { get; set; }
    }
}
