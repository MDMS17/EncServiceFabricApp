using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ReferenceRange : BackboneElement
    {
        public decimal? low { get; set; }
        public decimal? high { get; set; }
        public CodeableConcept type { get; set; }
        public List<CodeableConcept> appliesTo { get; set; }
        public Range age { get; set; }
        public string text { get; set; }
    }
}
