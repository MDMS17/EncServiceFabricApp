using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ReactionImmunization:BackboneElement
    {
        public DateTime? date { get; set;}
        public Reference detail { get; set; }
        public bool? reported { get; set; }
    }
}
