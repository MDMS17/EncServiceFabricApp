using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Workflow
{
    public class Referral : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<string> instantiatesUri { get; set; }
        public List<Reference > basedOn { get; set;}
        public List<Reference > replaces { get; set;}
        public Identifier requisition { get; set; }
        public string status { get; set; }
        public string intent { get; set; }
        public List<CodeableConcept> category { get; set; }
        public string priority { get; set; }
        public bool? doNotPerform { get; set; }
        public CodeableConcept code { get; set; }
        public List<CodeableConcept> orderDetail { get; set; }
        public QuantityReferral quantity { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Occurrence occurrence { get; set; }
        public AsNeeded asNeeded { get; set; }
        public DateTime? authoredOn { get; set; }
        public Reference requester { get; set; }
        public CodeableConcept performerType { get; set; }
        public List<Reference> performer { get; set; }
        public List<CodeableConcept> locationCode { get; set; }
        public List<Reference> locationReference { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference > reasonReference { get; set;}
        public List<Reference> insurance { get; set; }
        public List<Reference> supportingInfo { get; set; }
        public List<Reference> specimen { get; set; }
        public List<CodeableConcept> bodySite { get; set; }
        public List<Annotation> note { get; set; }
        public string patientInstruction { get; set; }
        public List<Reference> relevantHistory { get; set; }
    }
}
