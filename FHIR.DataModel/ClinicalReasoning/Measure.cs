using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.ClinicalReasoning
{
    public class Measure : DomainResource
    {
        public string url { get; set; }
        public List<Identifier> identifier { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public bool? experimental { get; set; }
        public Subject subject { get; set; }
        public DateTime? date { get; set; }
        public string publisher { get; set; }
        public List<ContactDetail> contact { get; set; }
        public string description { get; set; }
        public List<UsageContext> useContext { get; set; }
        public List<CodeableConcept> jurisdiction { get; set; }
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
        public string disclaimer { get; set; }
        public CodeableConcept scoring { get; set; }
        public CodeableConcept compositeScoring { get; set; }
        public List<CodeableConcept> type { get; set; }
        public string riskAdjustment { get; set; }
        public string rateAggregation { get; set; }
        public string rationale { get; set; }
        public string clinicalRecommendationStatement { get; set; }
        public CodeableConcept improvementNotation { get; set; }
        public List<string> definition { get; set; }
        public List<string> guidance { get; set; }
        public List<Group> group { get; set; }
        public List<SupplementalData> supplementalData { get; set; }
    }
}
