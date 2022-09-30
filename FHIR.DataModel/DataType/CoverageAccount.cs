using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class CoverageAccount : BackboneElement
    {
        public Reference coverage { get; set; }
        public uint? priority { get; set; }
    }
}
