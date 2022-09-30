using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class Coverage : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept type { get; set; }
        public Reference policyHolder { get; set; }
        public Reference subscriber { get; set; }
        public string subscriberId { get; set; }
        public Reference beneficiary { get; set; }
        public string dependent { get; set; }
        public CodeableConcept relationship { get; set; }
        public Period period { get; set; }
        public List<Reference> payor { get; set; }
        public List<ClassCoverage> _class { get;set;}
        public uint? order { get; set; }
        public string network { get; set; }
        public List<CostToBeneficiary> costToBeneficiary { get; set; }
        public bool? subrogation { get; set; }
        public List<Reference> contract { get; set; }
    }
}
