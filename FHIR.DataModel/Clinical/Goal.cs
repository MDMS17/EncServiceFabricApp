using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class Goal : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string lifecycleStatus { get; set; }
        public CodeableConcept achievementStatus { get; set; }
        public List<CodeableConcept> category { get; set; }
        public CodeableConcept priority { get; set; }
        public CodeableConcept description { get; set; }
        public Reference subject { get; set; }
        public Start start { get; set; }
        public List<Target> target { get; set; }
        public DateTime? statusDate { get; set; }
        public string statusReason { get; set; }
        public Reference expressedBy { get; set; }
        public List<Reference> addresses { get; set; }
        public List<Annotation> note { get; set; }
        public List<CodeableConcept> outcomeCode { get; set; }
        public List<Reference> outcomeReference { get; set; }

    }
}
