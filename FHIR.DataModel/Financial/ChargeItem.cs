using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class ChargeItem : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> definitionUri { get; set; }
        public string definitionCanonical { get; set; }
        public string status { get; set; }
        public List<Reference> partOf { get; set; }
        public CodeableConcept code { get; set; }
        public Reference subject { get; set; }
        public Reference context { get; set; }
        public Occurrence occurrence { get; set; }
        public List<Performer> performer { get; set; }
        public Reference performingOrganization { get; set; }
        public Reference requestingOrganization { get; set; }
        public Reference costCenter { get; set; }
        public decimal? quantity { get; set; }
        public List<CodeableConcept> bodySite { get; set; }
        public decimal? factorOverride { get; set; }
        public Money priceOverride { get; set; }
        public string overrideReason { get; set; }
        public Reference enterer { get; set; }
        public DateTime? enteredDate { get; set; }
        public List<CodeableConcept> reason { get; set; }
        public List<Reference> service { get; set; }
        public ProductChargeItem product { get; set; }
        public List<Reference> account { get; set; }
        public List<Annotation> note { get; set; }
        public List<Reference> supportingInformation { get; set; }
    }
}
