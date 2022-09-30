using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TargetPlanDefinition : BackboneElement
    {
        public CodeableConcept measure { get; set; }
        public DetailPlanDefinition detail { get; set; }
        public TimeSpan due { get; set; }
    }
}
