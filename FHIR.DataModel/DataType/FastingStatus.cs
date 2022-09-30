using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class FastingStatus
    {
        public CodeableConcept fastingStatusCodeableConcept { get; set; }
        public TimeSpan fastingStatusDuration { get; set; }
    }
}
