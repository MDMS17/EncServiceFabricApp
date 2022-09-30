using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class MedicationRequest : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept statusReason { get; set; }
        public string intent { get; set; }
        public List<CodeableConcept> category { get; set; }
        public string priority { get; set; }
        public bool? doNotPerform { get; set; }
        public Reported reported { get; set; }
        public Medication medication { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public List<Reference> supportingInformation { get; set; }
        public DateTime? authoredOn { get; set; }
        public Reference requester { get; set; }
        public Reference performer { get; set; }
        public CodeableConcept performerType { get; set; }
        public Reference recorder { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<string> instantiatesUri { get; set; }
        public List<Reference> basedOn { get; set; }
        public Identifier groupIdentifier { get; set; }
        public CodeableConcept courseOfTherapyType { get; set; }
        public List<Reference> insurance { get; set; }
        public List<Annotation> note { get; set; }
        public List<Dosage> dosageInstruction { get; set; }
        public DispenseRequest dispenseRequest { get; set; }
        public Substitution substitution { get; set; }
        public Reference priorPrescription { get; set; }
        public List<Reference> detectedIssue { get; set; }
        public List<Reference> eventHistory { get; set; }

    }
}
