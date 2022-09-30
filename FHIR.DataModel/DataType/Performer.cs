using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Performer : BackboneElement
    {
        public CodeableConcept function { get; set; }
        public Reference actor { get; set; }
        public Reference onBehalfOf { get; set; }
    }
}
