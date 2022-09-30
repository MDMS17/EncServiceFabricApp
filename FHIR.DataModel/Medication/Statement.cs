using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class MedicationStatement : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<Reference> basedOn { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> statusReason { get; set; }
        public CodeableConcept category { get; set; }
        public FHIR.DataModel.DataType.Medication medication { get; set; }
        public Reference subject { get; set; }
        public Reference context { get; set; }
        public EffectiveMedicationAdministration effective { get; set; }
        public DateTime? dateAsserted { get; set; }
        public Reference informationSource { get; set; }
        public List<Reference> derivedFrom { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Annotation> note { get; set; }
        public List<Dosage> dosage { get; set; }
    }
}
