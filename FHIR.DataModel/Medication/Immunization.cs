using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class Immunization : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept statusReason { get; set; }
        public CodeableConcept vaccineCode { get; set; }
        public Reference patient { get; set; }
        public Reference encounter { get; set; }
        public OccurrenceImmunization occurrence { get; set; }
        public DateTime? recorded { get; set; }
        public bool? primarySource { get; set; }
        public CodeableConcept reportOrigin { get; set; }
        public Reference location { get; set; }
        public Reference manufacturer { get; set; }
        public string lotNumber { get; set; }
        public DateTime? expirationDate { get; set; }
        public CodeableConcept site { get; set; }
        public CodeableConcept route { get; set; }
        public decimal? doseQuantity { get; set; }
        public List<PerformerDispense> performer { get; set; }
        public List<Annotation> note { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public bool? isSubpotent { get; set; }
        public List<CodeableConcept> subpotentReason { get; set; }
        public List<Education> education { get; set; }
        public List<CodeableConcept> programEligibility { get; set; }
        public CodeableConcept fundingSource { get; set; }
        public List<ReactionImmunization> reaction { get; set; }
        public List<ProtocolApplied> protocolApplied { get; set; }
    }
}
