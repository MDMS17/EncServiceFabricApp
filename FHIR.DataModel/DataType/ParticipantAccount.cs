using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ParticipantAccount
    {
        public CodeableConcept role { get; set; }
        public Reference actor { get; set; }
    }
}
