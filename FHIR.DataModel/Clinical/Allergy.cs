using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class AllergyIntolerance : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public CodeableConcept clinicalStatus { get; set; }
        public CodeableConcept verificationStatus { get; set; }
        public string type { get; set; }
        public List<string> category { get; set; }
        public string criticality { get; set; }
        public CodeableConcept code { get; set; }
        public Reference patient { get; set; }
        public Reference encounter { get; set; }
        public Onset onset { get; set; }
        public DateTime? recordedDate { get; set; }
        public Reference recorder { get; set; }
        public Reference asserter { get; set; }
        public DateTime? lastOccurrence { get; set; }
        public List<Annotation> note { get; set; }
        public List<Reaction> reaction { get; set; }

    }
}
