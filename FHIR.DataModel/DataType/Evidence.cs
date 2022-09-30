using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Evidence : BackboneElement
    {
        public List<CodeableConcept> code { get; set; }
        public List<Reference> detail { get; set; }
    }
}
