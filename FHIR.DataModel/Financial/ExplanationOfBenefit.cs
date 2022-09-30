using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Financial
{
    public class ExplanationOfBenefit:DomainResource
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
        public CodeableConcept fundReservedRequested { get; set; }
        public CodeableConcept fundReserve { get; set; }
        public List<RelatedClaim> related { get; set; }
        public Reference prescription { get; set; }
        public Reference originalPrescription { get; set; }
        public Payee payee { get; set; }
        public Reference referral { get; set; }
        public Reference facility { get; set; }
        public Reference claim { get; set; }
        public Reference claimResponse { get; set; }
        public string outcome { get; set; }
        public string description { get; set; }
        public List<string> preAuthRef { get; set; }
        public List<Period> preAuthRefPeriod { get; set; }
        public List<CareTeamClaim> careTeam { get; set; }
        public List<SupportingInfo> supportingInfo { get; set; }
        public List<Diagnosis> diagnosis { get; set; }
        public List<Procedure> procedure { get; set; }
        public uint? precedence { get; set;}
        public List<InsuranceExplanationOfBenefit> insurance { get; set; }
        public Accident accident { get; set; }
        public List<ItemExplanationOfBenefit> item { get; set; }
        public List<ItemExplanationOfBenefitAdd> addItem { get; set; }
        public List<Adjudication> adjudication { get; set; }
        public List<Total> total { get; set; }
        public Payment payment { get; set; }
        public CodeableConcept formCode { get; set; }
        public Attachment form { get; set; }
        public List<ProcessNote> processNote { get; set; }
        public Period benefitPeriod { get; set; }
        public List<BenefitBalance> benefitBalance { get; set; }
    }
}
