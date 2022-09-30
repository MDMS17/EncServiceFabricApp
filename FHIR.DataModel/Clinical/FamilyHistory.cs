using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class FamilyMemberHistory : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<String> instantiatesUri { get; set; }
        public string status { get; set; }
        public CodeableConcept dataAbsentReason { get; set; }
        public Reference patient { get; set; }
        public DateTime? date { get; set; }
        public string name { get; set; }
        public CodeableConcept relationship { get; set; }
        public CodeableConcept sex { get; set; }
        public Born born { get; set; }
        public Age age { get; set; }
        public bool? estimatedAge { get; set; }
        public Deceased deceased { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Annotation> note { get; set; }
        public List<Condition> condition { get; set; }
    }
}
