using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Reaction : BackboneElement
    {
        public CodeableConcept substance { get; set; }
        public List<CodeableConcept> manifestation { get; set; }
        public string description { get; set; }
        public DateTime? onset { get; set; }
        public string severity { get; set; }
        public CodeableConcept exposureRoute { get; set; }
        public List<Annotation> note { get; set; }
    }
}
