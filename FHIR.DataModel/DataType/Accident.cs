using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Accident:BackboneElement
    {
        public DateTime? date { get; set; }
        public CodeableConcept type { get; set; }
        public Location location { get; set; }
    }
}
