using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class InsuranceExplanationOfBenefit:BackboneElement 
    {
        public bool focal { get; set; }
        public Reference coverage { get; set; }
        public List<string> preAuthRef { get; set; }
    }
}
