using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ParticipantAppointment : BackboneElement
    {
        public List<CodeableConcept> type { get; set; }
        public Reference actor { get; set; }
        public string required { get; set; }
        public string status { get; set; }
        public Period period { get; set; }
    }
}
