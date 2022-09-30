using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ItemEligibilityResponse:BackboneElement 
    {
        public CodeableConcept category { get; set; }
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public Reference provider { get; set; }
        public bool? excluded { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CodeableConcept network { get; set; }
        public CodeableConcept unit { get; set; }
        public CodeableConcept term { get; set; }
        public List<BenefitEligibilityResponse> benefit { get; set; }
        public bool? authorizationRequired { get; set; }
        public List<CodeableConcept> authorizationSupporting { get; set; }
        public string authorizationUrl { get; set; }
    }
}
