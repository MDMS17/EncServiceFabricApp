using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Diagnostics
{
    public class Observation : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<Reference> basedOn { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> category { get; set; }
        public CodeableConcept code { get; set; }
        public Reference subject { get; set; }
        public List<Reference> focus { get; set; }
        public Reference encounter { get; set; }
        public Effective effective { get; set; }
        public DateTime? issued { get; set; }
        public List<Reference> performer { get; set; }
        public Value value { get; set; }
        public CodeableConcept dataAbsentReason { get; set; }
        public List<CodeableConcept> interpretation { get; set; }
        public List<Annotation> note { get; set; }
        public CodeableConcept bodySite { get; set; }
        public CodeableConcept method { get; set; }
        public Reference specimen { get; set; }
        public Reference device { get; set; }
        public ReferenceRange referenceRange { get; set; }
        public List<Reference> hasMember { get; set; }
        public List<Reference> derivedFrom { get; set; }
        public List<Component> component { get; set; }

    }
}
