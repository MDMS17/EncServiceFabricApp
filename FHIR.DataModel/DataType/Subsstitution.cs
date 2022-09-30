using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Subsstitution:BackboneElement
    {
        public bool wasSubstituted { get; set; }
        public CodeableConcept type { get; set; }
        public List<CodeableConcept> reason { get; set; }
        public List<Reference> responsibleParty { get; set; }
    }
}
