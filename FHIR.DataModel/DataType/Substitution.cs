using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Substitution : BackboneElement
    {
        public Allowed allowed { get; set; }
        public CodeableConcept reason { get; set; }
    }
}
