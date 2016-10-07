using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridProject
{
    public class Row
    {
        public Dictionary<string, object> Fields { get; set; }
        public List<Row> Children { get; set; }

        public Row()
        {
            this.Fields = new Dictionary<string, object>();
            this.Children = new List<Row>();
        }
    }
}
