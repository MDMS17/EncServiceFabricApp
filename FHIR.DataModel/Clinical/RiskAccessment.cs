using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class RiskAccessment : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public Reference basedOn { get; set; }
        public Reference parent { get; set; }
        public string status { get; set; }
        public CodeableConcept method { get; set; }
        public CodeableConcept code { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Occurrence occurrence { get; set; }
        public Reference condition { get; set; }
        public Reference performer { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Reference> basis { get; set; }
        public Prediction prediction { get; set; }
        public string mitigation { get; set; }
        public List<Annotation> note { get; set; }
    }
}
