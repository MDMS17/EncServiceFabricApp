using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.ClinicalReasoning
{
    public class PlanDefinition:DomainResource
    {
        public string url { get; set; }
        public List<Identifier> identifier { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public CodeableConcept type { get; set; }
        public string status { get; set; }
        public bool? experimental { get; set; }
        public Subject subject { get; set; }
        public DateTime? date { get; set; }
        public string publisher { get; set; }
        public List<ContactDetail> contact { get; set; }
        public string description { get; set; }
        public List<UsageContext> useContext { get; set; }
        public List<CodeableConcept> juridiction { get; set; }
        public string purpose { get; set; }
        public string usage { get; set; }
        public string copyright { get; set; }
        public DateTime? approvedDate { get; set; }
        public DateTime? lastReviewDate { get; set; }
        public Period effectivePeriod { get; set; }
        public List<CodeableConcept> topic { get; set; }
        public List<ContactDetail> author { get; set; }
        public List<ContactDetail> editor { get; set; }
        public List<ContactDetail> reviewer { get; set; }
        public List<ContactDetail> endorser { get; set; }
        public List<RelatedArtifact> relatedArtifact { get; set; }
        public List<string> library { get; set; }
        public List<Goal> goal { get; set; }
        public List<FHIR.DataModel.DataType.Action> action { get; set; }
    }
}
