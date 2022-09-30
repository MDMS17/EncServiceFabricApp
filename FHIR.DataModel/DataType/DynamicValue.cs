using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DynamicValue : BackboneElement
    {
        public string path { get; set; }
        public Expression expression { get; set; }
    }
}
