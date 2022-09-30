using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class BenefitBalance:BackboneElement 
    {
        public CodeableConcept category { get; set; }
        public bool? excluded { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CodeableConcept network { get; set; }
        public CodeableConcept unit { get; set; }
        public CodeableConcept term { get; set; }
        public List<Financial> financial { get; set; }
    }
}
