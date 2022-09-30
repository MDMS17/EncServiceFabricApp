using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.ClinicalReasoning
{
    public class MeasureReport:DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string measure { get; set; }
        public Reference subject { get; set; }
        public DateTime? date { get; set; }
        public Reference reporter { get; set; }
        public Period period { get; set; }
        public CodeableConcept improvementNotation { get; set; }
        public List<Group> group { get; set; }
        public List<Reference> evaluatedResource { get; set; }
    }
}
