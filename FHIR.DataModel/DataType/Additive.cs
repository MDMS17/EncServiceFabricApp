using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Additive
    {
        public CodeableConcept additiveCodeableConcept { get; set; }
        public Reference additiveReference { get; set; }
    }
}
