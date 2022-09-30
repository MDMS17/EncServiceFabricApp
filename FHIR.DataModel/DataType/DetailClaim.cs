using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DetailClaim:BackboneElement
    {
        public uint sequence { get; set; }
        public CodeableConcept revenue { get; set; }
        public CodeableConcept category { get; set; }
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public List<CodeableConcept> programCode { get; set; }
        public decimal? quantity { get; set; }
        public Money unitPrice { get; set; }
        public decimal? factor { get; set; }
        public Money net { get; set; }
        public List<Reference> udi { get; set; }
        public List<SubDetail> subDetail { get; set; }
    }
}
