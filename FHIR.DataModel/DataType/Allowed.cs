using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Allowed
    {
        public bool? allowedBoolean { get; set; }
        public CodeableConcept allowedCodeableConcept { get; set; }
    }
}
