using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ItemExplanationOfBenefit:BackboneElement 
    {
        public uint sequence { get; set; }
        public uint? careTeamSequence { get; set; }
        public uint? diagnosisSequence { get; set; }
        public uint? procedureSequence { get; set; }
        public uint? informationSequence { get; set; }
        public CodeableConcept revenue { get; set; }
        public CodeableConcept category { get; set; }
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public List<CodeableConcept> programCode { get; set; }
        public Serviced serviced { get; set; }
        public LocationItem location { get; set; }
        public decimal? quantity { get; set; }
        public Money unitPrice { get; set; }
        public decimal? factor { get; set; }
        public Money net { get; set; }
        public List<Reference> udi { get; set; }
        public CodeableConcept bodySite { get; set; }
        public List<CodeableConcept> subSite { get; set; }
        public List<Reference> encounter { get; set; }
        public List<uint> noteNumber { get; set; }
        public List<Adjudication> adjudication { get; set; }
        public List<DetailExplanationOfBenefit> detail { get; set; }
    }
}
