using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class MedicationAdministration : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> instantiates { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> statusReason { get; set; }
        public CodeableConcept category { get; set; }
        public FHIR.DataModel.DataType.Medication medication { get; set; }
        public Reference subject { get; set; }
        public Reference context { get; set; }
        public List<Reference> supportingInformation { get; set; }
        public EffectiveMedicationAdministration effective { get; set; }
        public List<PerformerDispense> performer { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public Reference request { get; set; }
        public List<Reference> device { get; set; }
        public List<Annotation> note { get; set; }
        public DosageMedicationAdministration dosage { get; set;}
        public List<Reference> eventHistory { get; set; }
    }
}
