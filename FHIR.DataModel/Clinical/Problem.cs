using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class Condition : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public CodeableConcept clinicalStatus { get; set; }
        public CodeableConcept verificationStatus { get; set; }
        public List<CodeableConcept> category { get; set; }
        public CodeableConcept severity { get; set; }
        public CodeableConcept code { get; set; }
        public List<CodeableConcept> bodySite { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Onset onset { get; set; }
        public Abatement abatement { get; set; }
        public DateTime? recordedDate { get; set; }
        public Reference recorder { get; set; }
        public Reference asserter { get; set; }
        public List<Stage> stage { get; set; }
        public List<Evidence> evidence { get; set; }
        public List<Annotation> note { get; set; }
    }
}
