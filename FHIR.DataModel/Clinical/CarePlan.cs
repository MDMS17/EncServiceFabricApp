using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Clinical
{
    public class CarePlan : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<string> instantiatesUri { get; set; }
        public List<Reference> basedOn { get; set; }
        public List<Reference> replaces { get; set; }
        public List<Reference> partOf { get; set; }
        public string status { get; set; }
        public string intent { get; set; }
        public List<CodeableConcept> category { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Period period { get; set; }
        public DateTime? created { get; set; }
        public Reference author { get; set; }
        public List<Reference> contributor { get; set; }
        public List<Reference> careTeam { get; set; }
        public List<Reference> addresses { get; set; }
        public List<Reference> supportingInfo { get; set; }
        public List<Reference> goal { get; set; }
        public List<Activity> activity { get; set;}
        public List<Annotation> note { get; set; }
    }
}
