using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class MedicationDispense : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public StatusReason statusReason { get; set; }
        public CodeableConcept category { get; set; }
        public Medication medication { get; set; }
        public Reference subject { get; set; }
        public Reference context { get; set; }
        public List<Reference> supportingInformation { get; set; }
        public List<PerformerDispense> performer { get; set; }
        public Reference location { get; set; }
        public List<Reference> authoringPrescription { get; set; }
        public CodeableConcept type { get; set; }
        public decimal? quantity { get; set; }
        public decimal? daysSupply { get; set; }
        public DateTime? whenPrepared { get; set; }
        public DateTime? whenHandedOver { get; set; }
        public Reference destination { get; set; }
        public List<Reference> receiver { get; set; }
        public List<Annotation> note { get; set; }
        public List<Dosage> dosageInstruction { get; set; }
        public Subsstitution substitution { get; set; }
        public List<Reference> detectedIssue { get; set; }
        public List<Reference> eventHistory { get; set; }
    }
}
