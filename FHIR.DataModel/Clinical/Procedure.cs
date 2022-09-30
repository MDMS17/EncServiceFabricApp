using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class Procedure : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<string> instantiatesUri { get; set; }
        public List<Reference> basedOn { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public CodeableConcept statusReason { get; set; }
        public CodeableConcept category { get; set; }
        public CodeableConcept code { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Performed performed { get; set; }
        public Reference recorder { get; set; }
        public Reference asserter { get; set; }
        public List<Performer> performer { get; set; }
        public Reference location { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<CodeableConcept> bodySite { get; set; }
        public CodeableConcept outcome { get; set; }
        public List<Reference> report { get; set; }
        public List<CodeableConcept> complication { get; set; }
        public List<Reference> complicationDetail { get; set; }
        public List<CodeableConcept> followUp { get; set; }
        public List<Annotation> note { get; set; }
        public List<FocalDevice> focalDevice { get; set; }
        public List<Reference> userReference { get; set; }
        public List<CodeableConcept> usedCode { get; set; }
    }
}
