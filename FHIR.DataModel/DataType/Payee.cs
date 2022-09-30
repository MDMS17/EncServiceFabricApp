using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Payee:BackboneElement
    {
        public CodeableConcept type { get; set; }
        public Reference party { get; set; }
    }
}
