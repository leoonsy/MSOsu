using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSOsu.Model
{
    public class ValuesColumn
    {
        public string ColumnName { get; set; }
        public double[] Values { get; set; }
        public ValuesColumn(string columnName, double[] values)
        {
            ColumnName = columnName;
            Values = values;
        }
    }
}
