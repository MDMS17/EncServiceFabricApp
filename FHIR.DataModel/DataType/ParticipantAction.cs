using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ParticipantAction:BackboneElement
    {
        public string type { get; set; }
        public CodeableConcept role { get; set; }
    }
}
