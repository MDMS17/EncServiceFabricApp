using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class InsuranceEligibilityResponse:BackboneElement 
    {
        public Reference coverage { get; set; }
        public bool? inforce { get; set; }
        public Period benefitPeriod { get; set; }
        public List<ItemEligibilityResponse> item { get; set; }
    }
}
