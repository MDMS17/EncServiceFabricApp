using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class LocationItem
    {
        public CodeableConcept locationCodeableConcept { get; set; }
        public Address locationAddress { get; set; }
        public Reference locationReference { get; set; }
    }
}
