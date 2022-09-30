using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Property : BackboneElement
    {
        public CodeableConcept type { get; set; }
        public List<Quantity> valueQuantity { get; set; }
        public List<CodeableConcept> valueCode { get; set; }
    }
}
