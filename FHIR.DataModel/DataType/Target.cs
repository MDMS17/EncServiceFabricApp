using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Target : BackboneElement
    {
        public CodeableConcept measure { get; set; }
        public DetailGoal detail { get; set; }
        public Due due { get; set; }
    }
}
