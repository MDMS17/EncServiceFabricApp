using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Financial:BackboneElement 
    {
        public CodeableConcept type { get; set; }
        public AllowedEligibilityResponse allowed { get; set; }
        public Used used { get; set; }
    }
}
