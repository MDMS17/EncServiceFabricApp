using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ItemExplanationOfBenefitAdd:BackboneElement 
    {
        public List<uint> itemSequence { get; set; }
        public List<uint> detailSequence { get; set; }
        public List<uint> subDetailSequence { get; set; }
        public List<Reference> provider { get; set; }
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public List<CodeableConcept> programCode { get; set; }
        public Serviced serviced { get; set; }
        public LocationItem location { get; set; }
        public decimal? quantity { get; set; }
        public Money unitPrice { get; set; }
        public decimal? factor { get; set; }
        public Money net { get; set; }
        public CodeableConcept bodySite { get; set; }
        public List<CodeableConcept> subSite { get; set; }
        public List<uint> noteNumber { get; set; }
        public List<Adjudication> adjudication { get; set; }
        public List<DetailExplanationOfBenefitAdd> detail { get; set; }

    }
}
