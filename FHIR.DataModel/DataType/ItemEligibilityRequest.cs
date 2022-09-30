using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ItemEligibilityRequest:BackboneElement 
    {
        public uint? supportingInfoSequence { get; set; }
        public CodeableConcept category { get; set; }
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public Reference provider { get; set; }
        public decimal? quantity { get; set; }
        public Money unitPrice { get; set; }
        public Reference facility { get; set; }
        public List<DiagnosisEligibilityRequest> diagnosis { get; set; }
        public List<Reference> detail { get; set; }
    }
}
