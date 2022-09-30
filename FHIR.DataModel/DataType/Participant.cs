using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Participant : BackboneElement
    {
        public List<CodeableConcept> role { get; set; }
        public Reference member { get; set; }
        public Reference onBehalfOf { get; set; }
        public Period period { get; set; }
    }
}
