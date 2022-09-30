using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class Claim:DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept type { get; set; }
        public CodeableConcept subType { get; set; }
        public string use { get; set; }
        public Reference patient { get; set; }
        public Period billablePeriod { get; set; }
        public DateTime created { get; set; }
        public Reference enterer { get; set; }
        public Reference insurer { get; set; }
        public Reference provider { get; set; }
        public CodeableConcept priority { get; set; }
        public CodeableConcept fundsReserve { get; set; }
        public List<RelatedClaim> related { get; set; }
        public Reference prescription { get; set; }
        public Reference originalPrescription { get; set; }
        public Payee payee { get; set; }
        public Reference referral { get; set; }
        public Reference facility { get; set; }
        public List<CareTeamClaim> careTeam { get; set; }
        public List<SupportingInfo> supportingInfo { get; set; }
        public List<Diagnosis> diagnosis { get; set; }
        public List<Procedure> procedure { get; set; }
        public List<Insurance> insurance { get; set; }
        public Accident accident { get; set; }
        public List<ItemClaim> item { get; set; }
        public Money total { get; set; }
    }
}
