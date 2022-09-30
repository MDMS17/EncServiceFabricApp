using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class CoverageEligibilityRequest : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept priority { get; set; }
        public List<string> purpose { get; set; }
        public Reference patient { get; set; }
        public Serviced serviced { get; set; }
        public DateTime created { get; set; }
        public Reference enterer { get; set; }
        public Reference provider { get; set; }
        public Reference insurer { get; set; }
        public Reference facility { get; set; }
        public List<SupportingInfoEligibilityRequest> supportingInfo { get; set; }
        public List<InsuranceEligibilityRequest> insurance { get; set; }
        public List<ItemEligibilityRequest> item { get; set; }
    }
}
