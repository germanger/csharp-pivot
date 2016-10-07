using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotDataProject
{
    class Row
    {
        public Dictionary<string, object> Fields { get; set; }
        public List<Row> Children { get; set; }
    }
}
