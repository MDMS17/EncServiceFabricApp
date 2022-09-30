using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class Account : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept type { get; set; }
        public string name { get; set; }
        public List<Reference> subject { get; set; }
        public Period servicePeriod { get; set; }
        public List<CoverageAccount> coverage { get; set; }
        public Reference owner { get; set; }
        public string description { get; set; }
        public List<Guarantor> guarantor { get; set; }
        public Reference partOf { get; set; }

    }
}
