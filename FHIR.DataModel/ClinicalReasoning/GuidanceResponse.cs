using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.ClinicalReasoning
{
    public class GuidanceResponse : DomainResource
    {
        public Identifier requestIDentifier { get; set; }
        public List<Identifier> identifier { get; set; }
        public Module module { get; set; }
        public string status { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public DateTime? occurrenceDateTime { get; set; }
        public Reference performer { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Annotation> note { get; set; }
        public List<Reference> evaluationMessage { get; set; }
        public Reference outputParameters { get; set; }
        public Reference result { get; set; }
        public List<DataRequirement> dataRequirement { get; set; }
    }
}
