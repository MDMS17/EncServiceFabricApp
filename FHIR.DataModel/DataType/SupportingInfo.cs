using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SupportingInfo:BackboneElement
    {
        public uint sequence { get; set; }
        public CodeableConcept category { get; set; }
        public CodeableConcept code { get; set; }
        public TimingClaim timing { get; set; }
        public ValueClaim value { get; set; }
        public CodeableConcept reason { get; set; }
    }
}
