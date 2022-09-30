using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class Invoice : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public string cancelledReason { get; set; }
        public CodeableConcept type { get; set; }
        public Reference subject { get; set; }
        public Reference recipient { get; set; }
        public DateTime? date { get; set; }
        public List<ParticipantAccount> participant { get; set; }
        public Reference issuer { get; set; }
        public Reference account { get; set; }
        public List<LineItem> lineItem { get; set; }
        public List<PriceComponent> totalPriceComponent { get; set; }
        public Money totalNet { get; set; }
        public Money totalGross { get; set; }
        public string paymentTerm { get; set; }
        public List<Annotation> note { get; set; }
    }
}
