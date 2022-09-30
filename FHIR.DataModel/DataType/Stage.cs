using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Stage : BackboneElement
    {
        public CodeableConcept summary { get; set; }
        public List<Reference> assessment { get; set; }
        public CodeableConcept type { get; set; }
    }
}
