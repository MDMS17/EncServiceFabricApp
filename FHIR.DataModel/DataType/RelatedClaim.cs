using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class RelatedClaim:BackboneElement
    {
        public Reference claim { get; set; }
        public CodeableConcept relationship { get; set; }
        public Identifier reference { get; set; }
    }
}
