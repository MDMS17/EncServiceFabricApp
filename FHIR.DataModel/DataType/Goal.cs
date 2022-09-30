using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Goal:BackboneElement 
    {
        public CodeableConcept category { get; set; }
        public CodeableConcept description { get; set; }
        public CodeableConcept priority { get; set; }
        public CodeableConcept start { get; set; }
        public List<CodeableConcept> addresses { get; set; }
        public List<RelatedArtifact> documentation { get; set; }
        public List<TargetPlanDefinition> target { get; set; }
    }
}
