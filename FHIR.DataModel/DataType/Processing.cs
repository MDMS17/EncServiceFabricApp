using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Processing : BackboneElement
    {
        public string description { get; set; }
        public CodeableConcept procedure { get; set; }
        public List<Reference> additive { get; set; }
        public Time time { get; set; }
    }
}
