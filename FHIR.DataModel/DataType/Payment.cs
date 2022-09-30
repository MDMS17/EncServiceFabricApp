using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Payment:BackboneElement 
    {
        public CodeableConcept type { get; set; }
        public Money adjustment { get; set; }
        public CodeableConcept adjustmentReason { get; set; }
        public DateTime? date { get; set; }
        public Money amount { get; set; }
        public Identifier identifier { get; set; }
    }
}
