using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Activity : BackboneElement
    {
        public List<CodeableConcept> outcomeCodeableConcept { get; set; }
        public List<Reference> outcomeReference { get; set; }
        public List<Annotation> progress { get; set; }
        public Reference reference { get; set; }
        public Detail detail { get; set; }
    }
}
