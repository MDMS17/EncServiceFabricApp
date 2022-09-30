using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class StatusReason
    {
        public CodeableConcept statusReasonCodeableConcept { get; set; }
        public Reference statusReasonReference { get; set; }
    }
}
