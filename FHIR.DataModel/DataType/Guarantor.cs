using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Guarantor :BackboneElement 
    {
        public Reference party { get; set; }
        public bool? onHold { get; set; }
        public Period period { get; set; }
    }
}
