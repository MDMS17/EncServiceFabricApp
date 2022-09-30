namespace EncDataModel.MCPDIP
{
    public class ExcelPcpaKaiser
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string Npi { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelPcpa
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string Npi { get; set; }
        public string NpiSourceType { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelAppeal
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string AppealId { get; set; }
        public string RecordType { get; set; }
        public string ParentGrievanceId { get; set; }
        public string ParentAppealId { get; set; }
        public string AppealReceivedDate { get; set; }
        public string NoticeOfActionDate { get; set; }
        public string AppealType { get; set; }
        public string BenefitType { get; set; }
        public string AppealResolutionStatusIndicator { get; set; }
        public string AppealResolutionDate { get; set; }
        public string PartiallyOverturnIndicator { get; set; }
        public string ExpeditedIndicator { get; set; }
        public string ExtractionDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelCocIpa
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string CocId { get; set; }
        public string RecordType { get; set; }
        public string ParentCocId { get; set; }
        public string CocReceivedDate { get; set; }
        public string CocType { get; set; }
        public string BenefitType { get; set; }
        public string CocDispositionIndicator { get; set; }
        public string CocExpirationDate { get; set; }
        public string CocDenialReasonIndicator { get; set; }
        public string SubmittingProviderNpi { get; set; }
        public string CocProviderNpi { get; set; }
        public string ProviderTaxonomy { get; set; }
        public string ExtractionDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelCoc
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string CocId { get; set; }
        public string RecordType { get; set; }
        public string ParentCocId { get; set; }
        public string CocReceivedDate { get; set; }
        public string CocType { get; set; }
        public string BenefitType { get; set; }
        public string CocDispositionIndicator { get; set; }
        public string CocExpirationDate { get; set; }
        public string CocDenialReasonIndicator { get; set; }
        public string SubmittingProviderNpi { get; set; }
        public string CocProviderNpi { get; set; }
        public string ProviderTaxonomy { get; set; }
        public string MerExemptionId { get; set; }
        public string ExemptionToEnrollmentDenialCode { get; set; }
        public string ExemptionToEnrollmentDenialDate { get; set; }
        public string MerCocDispositionIndicator { get; set; }
        public string MerCocDispositionDate { get; set; }
        public string ReasonMerCocNotMetIndicator { get; set; }
        public string ExtractionDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelGrievance
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string GrievanceId { get; set; }
        public string RecordType { get; set; }
        public string ParentGrievanceId { get; set; }
        public string GrievanceReceivedDate { get; set; }
        public string GrievanceType { get; set; }
        public string BenefitType { get; set; }
        public string ExemptIndicator { get; set; }
        public string ExtractionDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelOon
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string OonId { get; set; }
        public string RecordType { get; set; }
        public string ParentOonId { get; set; }
        public string OonRequestReceivedDate { get; set; }
        public string ReferralRequestReasonIndicator { get; set; }
        public string OonResolutionStatusIndicator { get; set; }
        public string OonRequestResolvedDate { get; set; }
        public string PartialApprovalExplanation { get; set; }
        public string SpecialistProviderNpi { get; set; }
        public string ProviderTaxonomy { get; set; }
        public string ServiceLocationAddressLine1 { get; set; }
        public string ServiceLocationAddressLine2 { get; set; }
        public string ServiceLocationCity { get; set; }
        public string ServiceLocationState { get; set; }
        public string ServiceLocationZip { get; set; }
        public string ServiceLocationCountry { get; set; }
        public string ExtractionDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelGrievanceIEHP
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string GrievanceId { get; set; }
        public string RecordType { get; set; }
        public string ParentGrievanceId { get; set; }
        public string GrievanceReceivedDate { get; set; }
        public string GrievanceType { get; set; }
        public string BenefitType { get; set; }
        public string ExemptIndicator { get; set; }
        public string CaseNumber { get; set; }
        public string CaseStatus { get; set; }
        public string DataSource { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ExcelAppealIEHP
    {
        public string PlanCode { get; set; }
        public string Cin { get; set; }
        public string AppealId { get; set; }
        public string RecordType { get; set; }
        public string ParentGrievanceId { get; set; }
        public string ParentAppealId { get; set; }
        public string AppealReceivedDate { get; set; }
        public string NoticeOfActionDate { get; set; }
        public string AppealType { get; set; }
        public string BenefitType { get; set; }
        public string AppealResolutionStatusIndicator { get; set; }
        public string AppealResolutionDate { get; set; }
        public string PartiallyOverturnIndicator { get; set; }
        public string ExpeditedIndicator { get; set; }
        public string CaseNumber { get; set; }
        public string CaseStatus { get; set; }
        public string DataSource { get; set; }
        public string ErrorMessage { get; set; }
    }
}
