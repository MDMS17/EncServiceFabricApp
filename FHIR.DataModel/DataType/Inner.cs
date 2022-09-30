using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Inner : BackboneElement 
    {
        public int? start { get; set; }
        public int? end { get; set; }
    }
}
