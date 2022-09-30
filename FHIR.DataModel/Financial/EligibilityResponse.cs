using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class CoverageEligibilityResponse:DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public List<string> purpose { get; set; }
        public Reference patient { get; set; }
        public Serviced serviced { get; set; }
        public DateTime created { get; set; }
        public Reference requestor { get; set; }
        public Reference request { get; set; }
        public string outcome { get; set; }
        public string disposition { get; set; }
        public Reference insurer { get; set; }
        public List<InsuranceEligibilityResponse> insurance { get; set; }
        public string preAuthRef { get; set; }
        public CodeableConcept form { get; set; }
        public List<ErrorEligibilityResponse> error { get; set; }

    }
}
