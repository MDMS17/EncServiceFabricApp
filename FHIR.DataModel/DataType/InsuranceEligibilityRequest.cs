using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class InsuranceEligibilityRequest:BackboneElement 
    {
        public bool? focal { get; set; }
        public Reference coverage { get; set; }
        public string businessArrangement { get; set; }
    }
}
