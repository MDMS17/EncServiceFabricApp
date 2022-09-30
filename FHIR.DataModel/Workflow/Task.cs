using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Workflow
{
    public class Task : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string instantiatesCanonical { get; set; }
        public string instantiatesUri { get; set; }
        public List<Reference> basedOn { get; set; }
        public Identifier groupIdentifier { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public CodeableConcept statusReason { get; set; }
        public CodeableConcept businessStatus { get; set; }
        public string intent { get; set; }
        public string priority { get; set; }
        public CodeableConcept code { get; set; }
        public string description { get; set; }
        public Reference focus { get; set; }
        public Reference _for{ get; set; }
        public Reference encounter { get; set; }
        public Period executionPeriod { get; set; }
        public DateTime? authoredOn { get; set; }
        public DateTime? lastModified { get; set; }
        public Reference requester { get; set; }
        public List<CodeableConcept> performerType { get; set; }
        public Reference owner { get; set; }
        public Reference location { get; set; }
        public CodeableConcept reasonCode { get; set; }
        public Reference reasonReference { get; set; }
        public List<Reference> insurance { get; set; }
        public List<Annotation> note { get; set; }
        public List<Reference> relevantHistory { get; set; }
        public Restriction restriction { get; set; }
        public List<TaskInputOutput> input { get; set; }
        public List<TaskInputOutput> output { get; set; }

    }
}
