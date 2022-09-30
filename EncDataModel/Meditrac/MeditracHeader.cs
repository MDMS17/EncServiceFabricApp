using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Meditrac
{
    public class MeditracHeader
    {
        public string BillingTaxonomyCode { get; set; }
        public string BillingLastName { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingMiddleInitial { get; set; }
        public string BillingNPI { get; set; }
        public string BillingAddress { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingTaxId { get; set; }
        public string ClaimFilingInd { get; set; }
        public string SubscriberLastName { get; set; }
        public string SubscriberFirstName { get; set; }
        public string SubscriberMiddleInitial { get; set; }
        public string SubscriberIdQual { get; set; }
        public string CIN { get; set; }
        public string HICN { get; set; }
        public string SubscriberAddress { get; set; }
        public string SubscriberAddress2 { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberState { get; set; }
        public string SubscriberZip { get; set; }
        public string SubscriberDateOfBirth { get; set; }
        public string SubscriberGender { get; set; }
        public int ClaimId { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string FacilityCode { get; set; }
        public string ClaimType { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string ProviderSignatureInd { get; set; }
        public string ProviderAssignmentInd { get; set; }
        public string BenefitAssignmentInd { get; set; }
        public string ReleaseOfInformationInd { get; set; }
        public string PatientSignatureInd { get; set; }
        public string DelayReasonCode { get; set; }
        public string AdmissionDate { get; set; }
        public string ContractTypeCode { get; set; }
        public decimal? ContractAmount { get; set; }
        public decimal? PatientPaidAmount { get; set; }
        public string ExternalClaimId { get; set; }
        public string MeditracSubmissionNumber { get; set; }
        public string AuthorizationNumber { get; set; }
        public string PayerControlNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string ReferringTaxonomyCode { get; set; }
        public string ReferringLastName { get; set; }
        public string ReferringFirstName { get; set; }
        public string ReferringMiddleInitial { get; set; }
        public string ReferringNPI { get; set; }
        public string ReferringAddress { get; set; }
        public string ReferringAddress2 { get; set; }
        public string ReferringCity { get; set; }
        public string ReferringState { get; set; }
        public string ReferringZip { get; set; }
        public string ReferringTaxId { get; set; }
        public string RenderingLastName { get; set; }
        public string RenderingFirstName { get; set; }
        public string RenderingMiddleInitial { get; set; }
        public string RenderingNPI { get; set; }
        public string RenderingTaxonomyCode { get; set; }
        public string RenderingAddress { get; set; }
        public string RenderingAddress2 { get; set; }
        public string RenderingCity { get; set; }
        public string RenderingState { get; set; }
        public string RenderingZip { get; set; }
        public string RenderingTaxId { get; set; }
        public string ServiceFacilityLastName { get; set; }
        public string ServiceFAcilityAddress { get; set; }
        public string ServiceFacilityAddress2 { get; set; }
        public string ServiceFacilityCity { get; set; }
        public string ServiceFacilityState { get; set; }
        public string ServiceFacilityZip { get; set; }
        public decimal? COBPayerPaidAmount { get; set; }
        public decimal? COBTotalNonCoveredAmount { get; set; }
        public decimal? RemainingPatientLiabilityAmount { get; set; }
        public string DischargeHour { get; set; }
        public string StatementDateFrom { get; set; }
        public string StatementDateTo { get; set; }
        public string AdmissionTypeCode { get; set; }
        public string AdmissionSourceCode { get; set; }
        public string PatientStatusCode { get; set; }
        public string AttLastName { get; set; }
        public string AttFirstName { get; set; }
        public string AttMiddleInitial { get; set; }
        public string AttNPI { get; set; }
        public string AttTaxonomyCode { get; set; }
        public string AttAddress { get; set; }
        public string AttAddress2 { get; set; }
        public string AttCity { get; set; }
        public string AttState { get; set; }
        public string AttZip { get; set; }
        public string AttTaxId { get; set; }
        public string OprLastName { get; set; }
        public string OprFirstName { get; set; }
        public string OprMiddleInitial { get; set; }
        public string OprNPI { get; set; }
        public string OthLastName { get; set; }
        public string OthFirstName { get; set; }
        public string OthMiddleInitial { get; set; }
        public string OthNPI { get; set; }
        public string GroupNumber { get; set; } //need to bring in group number, in case, we cannot get filing indicator from claimcobdata
        public string MedicaidID { get; set; }
    }
}
