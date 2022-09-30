using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.M834
{
    public class M834AdditionalName
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string NameQualifier { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string IdQualifier { get; set; }
        public string NameId { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Ethnicity { get; set; }
        public string EthnicityClassificationCode { get; set; }
        public string Citizenship { get; set; }
        public string EthnicityCollectionCode { get; set; }
        public string ContactName { get; set; }
        public string ContactQualifier1 { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactQualifier2 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactQualifier3 { get; set; }
        public string ContactNumber3 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressSTate { get; set; }
        public string AddressZip { get; set; }
        public string AddressCountry { get; set; }
        public string AddressCountrySubCode { get; set; }
    }
    public class M834Detail
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string ConditionCode { get; set; }
        public string RelationshipCode { get; set; }
        public string MaintenanceTypeCode { get; set; }
        public string MaintenanceReasonCode { get; set; }
        public string BenefitStatusCode { get; set; }
        public string MedicarePlanCode { get; set; }
        public string MedicareEligibilityReasonCode { get; set; }
        public string COBRACode { get; set; }
        public string EmploymentStatusCode { get; set; }
        public string StudentStatusCode { get; set; }
        public string HandicapInd { get; set; }
        public string MemberDeathDate { get; set; }
        public string ConfidentialityCode { get; set; }
        public string BirthSequenceNumber { get; set; }
        public string SubscriberId { get; set; }
        public string PolicyNumber { get; set; }
        public string MemberQualifier { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberPrefix { get; set; }
        public string MemberSuffix { get; set; }
        public string MemberIdQualifier { get; set; }
        public string MemberId { get; set; }
        public string CommunicationQualifier1 { get; set; }
        public string CommunicationNumber1 { get; set; }
        public string CommunicationQualifier2 { get; set; }
        public string CommunicationNumber2 { get; set; }
        public string CommunicationQualifier3 { get; set; }
        public string CommunicationNumber3 { get; set; }
        public string MemberAddress { get; set; }
        public string MemberAddress2 { get; set; }
        public string MemberCity { get; set; }
        public string MemberState { get; set; }
        public string MemberZip { get; set; }
        public string MemberCountry { get; set; }
        public string MemberLocationQualifier { get; set; }
        public string MemberLocationId { get; set; }
        public string MemberCountrySubCode { get; set; }
        public string MemberBirthDate { get; set; }
        public string MemberGender { get; set; }
        public string MemberMaritalStatus { get; set; }
        public string MemberEthnicity { get; set; }
        public string MemberCitizenship { get; set; }
        public string MemberEthnicityClassificationCode { get; set; }
        public string MemberEthnicityCollectionCode { get; set; }
        public string MemberIncomeFrequencyCode { get; set; }
        public string MemberIncome { get; set; }
        public string MemberWorkHours { get; set; }
        public string MemberWorkDepartment { get; set; }
        public string MemberSalaryGrade { get; set; }
        public string MemberHealthCode { get; set; }
        public string MemberHeight { get; set; }
        public string MemberWeight { get; set; }
    }
    public class M834DisabilityInfo
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string DisabilityTypeCode { get; set; }
        public string ProductIdQualifier { get; set; }
        public string DiagnosisCode { get; set; }
        public string DisabilityStartDate { get; set; }
        public string DisabilityEndDate { get; set; }
    }
    public class M834EmploymentClass
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string EmploymentClassCode1 { get; set; }
        public string EmploymentClassCode2 { get; set; }
        public string EmploymentClassCode3 { get; set; }
    }
    public class M834File
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionPurposeCode { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string TransactionTimeCode { get; set; }
        public string TransactionActionCode { get; set; }
        public string TransactionPolicyNumber { get; set; }
        public string EffectiveDate { get; set; }
        public string ReportStartDate { get; set; }
        public string ReportEndDate { get; set; }
        public string MaintenanceEffectiveDate { get; set; }
        public string EnrollmentDate { get; set; }
        public string PaymentDate { get; set; }
        public string DependentQuantity { get; set; }
        public string EmployeeQuantity { get; set; }
        public string TotalQuantity { get; set; }
        public string SponsorName { get; set; }
        public string SponsorIdQualifier { get; set; }
        public string SponsorId { get; set; }
        public string PayerName { get; set; }
        public string PayerIdQualifier { get; set; }
        public string PayerId { get; set; }
        public string Broker1Name { get; set; }
        public string Broker1IdQualifier { get; set; }
        public string Broker1Id { get; set; }
        public string Broker1AccountNumber { get; set; }
        public string Broker1AccountNumber2 { get; set; }
        public string Broker2Name { get; set; }
        public string Broker2IdQualifier { get; set; }
        public string Broker2Id { get; set; }
        public string Broker2AccountNumber { get; set; }
        public string Broker2AccountNumber2 { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class M834HCCOBInfo
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string HCId { get; set; }
        public string SequenceCode { get; set; }
        public string MemberGroupNumber { get; set; }
        public string CobCode { get; set; }
        public string ServiceTypeCode { get; set; }
        public string CobQualifier1 { get; set; }
        public string CobPolicyNumber1 { get; set; }
        public string CobQualifier2 { get; set; }
        public string CobPolicyNumber2 { get; set; }
        public string CobQualifier3 { get; set; }
        public string CobPolicyNumber3 { get; set; }
        public string CobQualifier4 { get; set; }
        public string CobPolicyNumber4 { get; set; }
        public string CobBeginDate { get; set; }
        public string CobEndDate { get; set; }
        public string Entity1Qualifier { get; set; }
        public string Entity1LastName { get; set; }
        public string Entity1IdQualifier { get; set; }
        public string Entity1Id { get; set; }
        public string Entity1Address { get; set; }
        public string Entity1Address2 { get; set; }
        public string Entity1AddressCity { get; set; }
        public string Entity1AddressState { get; set; }
        public string Entity1AddressZip { get; set; }
        public string Entity1AddressCountry { get; set; }
        public string Entity1AddressCountrySubCode { get; set; }
        public string Entity1ContactQualifier { get; set; }
        public string Entity1ContactNumber { get; set; }
        public string Entity2Qualifier { get; set; }
        public string Entity2LastName { get; set; }
        public string Entity2IdQualifier { get; set; }
        public string Entity2Id { get; set; }
        public string Entity2Address { get; set; }
        public string Entity2Address2 { get; set; }
        public string Entity2AddressCity { get; set; }
        public string Entity2AddressState { get; set; }
        public string Entity2AddressZip { get; set; }
        public string Entity2AddressCountry { get; set; }
        public string Entity2AddressCountrySubCode { get; set; }
        public string Entity2ContactQualifier { get; set; }
        public string Entity2ContactNumber { get; set; }
        public string Entity3Qualifier { get; set; }
        public string Entity3LastName { get; set; }
        public string Entity3IdQualifier { get; set; }
        public string Entity3Id { get; set; }
        public string Entity3Address { get; set; }
        public string Entity3Address2 { get; set; }
        public string Entity3AddressCity { get; set; }
        public string Entity3AddressState { get; set; }
        public string Entity3AddressZip { get; set; }
        public string Entity3AddressCountry { get; set; }
        public string Entity3AddressCountrySubCode { get; set; }
        public string Entity3ContactQualifier { get; set; }
        public string Entity3ContactNumber { get; set; }
    }
    public class M834HCProviderInfo
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string HCId { get; set; }
        public string SequenceNumber { get; set; }
        public string ProviderQualifier { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderMiddleName { get; set; }
        public string ProviderPrefix { get; set; }
        public string ProviderSuffix { get; set; }
        public string ProviderIdQualifier { get; set; }
        public string ProviderId { get; set; }
        public string EntityRelationshipCode { get; set; }
        public string ProviderAddress1 { get; set; }
        public string ProviderAddress12 { get; set; }
        public string ProviderAddress2 { get; set; }
        public string ProviderAddress22 { get; set; }
        public string ProviderAddressCity { get; set; }
        public string ProviderAddressState { get; set; }
        public string ProviderAddressZip { get; set; }
        public string ProviderAddressCountry { get; set; }
        public string ProviderAddressCountrySubCode { get; set; }
        public string ProviderContact1Qualifier1 { get; set; }
        public string ProviderContact1Number1 { get; set; }
        public string ProviderContact1Qualifier2 { get; set; }
        public string ProviderContact1Number2 { get; set; }
        public string ProviderContact1Qualifier3 { get; set; }
        public string ProviderContact1Number3 { get; set; }
        public string ProviderContact2Qualifier1 { get; set; }
        public string ProviderContact2Number1 { get; set; }
        public string ProviderContact2Qualifier2 { get; set; }
        public string ProviderContact2Number2 { get; set; }
        public string ProviderContact2Qualifier3 { get; set; }
        public string ProviderContact2Number3 { get; set; }
        public string ProviderChangeCode { get; set; }
        public string ProviderChangeQualifier { get; set; }
        public string ProviderChangeDate { get; set; }
        public string ProviderChangeReasonCode { get; set; }
    }
    public class M834HealthCoverage
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string HCId { get; set; }
        public string MaintenanceTypeCode { get; set; }
        public string InsuranceLineCode { get; set; }
        public string PlanCoverageDescription { get; set; }
        public string CoverageLevelCode { get; set; }
        public string LateEnrollmentInd { get; set; }
        public string DateQualifier1 { get; set; }
        public string DateBegin1 { get; set; }
        public string DateEnd1 { get; set; }
        public string DateQualifier2 { get; set; }
        public string DateBegin2 { get; set; }
        public string DateEnd2 { get; set; }
        public string DateQualifier3 { get; set; }
        public string DateBegin3 { get; set; }
        public string DateEnd3 { get; set; }
        public string DateQualifier4 { get; set; }
        public string DateBegin4 { get; set; }
        public string DateEnd4 { get; set; }
        public string DateQualifier5 { get; set; }
        public string DateBegin5 { get; set; }
        public string DateEnd5 { get; set; }
        public string DateQualifier6 { get; set; }
        public string DateBegin6 { get; set; }
        public string DateEnd6 { get; set; }
        public string AmountQualifier1 { get; set; }
        public string Amount1 { get; set; }
        public string AmountQualifier2 { get; set; }
        public string Amount2 { get; set; }
        public string AmountQualifier3 { get; set; }
        public string Amount3 { get; set; }
        public string AmountQualifier4 { get; set; }
        public string Amount4 { get; set; }
        public string AmountQualifier5 { get; set; }
        public string Amount5 { get; set; }
        public string AmountQualifier6 { get; set; }
        public string Amount6 { get; set; }
        public string AmountQualifier7 { get; set; }
        public string Amount7 { get; set; }
        public string AmountQualifier8 { get; set; }
        public string Amount8 { get; set; }
        public string AmountQualifier9 { get; set; }
        public string Amount9 { get; set; }
        public string PolicyQualifier01 { get; set; }
        public string PolicyNumber01 { get; set; }
        public string PolicyQualifier02 { get; set; }
        public string PolicyNumber02 { get; set; }
        public string PolicyQualifier03 { get; set; }
        public string PolicyNumber03 { get; set; }
        public string PolicyQualifier04 { get; set; }
        public string PolicyNumber04 { get; set; }
        public string PolicyQualifier05 { get; set; }
        public string PolicyNumber05 { get; set; }
        public string PolicyQualifier06 { get; set; }
        public string PolicyNumber06 { get; set; }
        public string PolicyQualifier07 { get; set; }
        public string PolicyNumber07 { get; set; }
        public string PolicyQualifier08 { get; set; }
        public string PolicyNumber08 { get; set; }
        public string PolicyQualifier09 { get; set; }
        public string PolicyNumber09 { get; set; }
        public string PolicyQualifier10 { get; set; }
        public string PolicyNumber10 { get; set; }
        public string PolicyQualifier11 { get; set; }
        public string PolicyNumber11 { get; set; }
        public string PolicyQualifier12 { get; set; }
        public string PolicyNumber12 { get; set; }
        public string PolicyQualifier13 { get; set; }
        public string PolicyNumber13 { get; set; }
        public string PolicyQualifier14 { get; set; }
        public string PolicyNumber14 { get; set; }
        public string PriorCoverageMonthCount { get; set; }
        public string PlanCoverageDescription1 { get; set; }
        public string IdCardTypeCode1 { get; set; }
        public string IdCardCount1 { get; set; }
        public string ActionCode1 { get; set; }
        public string PlanCoverageDescription2 { get; set; }
        public string IdCardTypeCode2 { get; set; }
        public string IdCardCount2 { get; set; }
        public string ActionCode2 { get; set; }
        public string PlanCoverageDescription3 { get; set; }
        public string IdCardTypeCode3 { get; set; }
        public string IdCardCount3 { get; set; }
        public string ActionCode3 { get; set; }
    }
    public class M834Language
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string IdQualifier { get; set; }
        public string LanguageId { get; set; }
        public string LanguageDescription { get; set; }
        public string LanguageUsageInd { get; set; }
    }
    public class M834MemberLevelDate
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string DateQualifier { get; set; }
        public string MemberLevelDate { get; set; }
    }
    public class M834PolicyAmount
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string AmountQualifier { get; set; }
        public string ContractAmount { get; set; }
    }
    public class M834ReportingCategory
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string SequenceNumber { get; set; }
        public string ParticipantName { get; set; }
        public string ReferenceIdQualifier { get; set; }
        public string ReferenceId { get; set; }
        public string ReportBeginDate { get; set; }
        public string ReportEndDate { get; set; }
    }
    public class M834SubId
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string SubIdQualifier { get; set; }
        public string SubId { get; set; }
    }
    public class Elig834
    {
        public Elig834()
        {
            m834additionalnames = new List<M834AdditionalName>();
            m834details = new List<M834Detail>();
            m834disabilityinfos = new List<M834DisabilityInfo>();
            m834employmentclasses = new List<M834EmploymentClass>();
            m834hccobinfos = new List<M834HCCOBInfo>();
            m834hcproviderinfos = new List<M834HCProviderInfo>();
            m834healthcoverages = new List<M834HealthCoverage>();
            m834languages = new List<M834Language>();
            m834memberleveldates = new List<M834MemberLevelDate>();
            m834policyamounts = new List<M834PolicyAmount>();
            m834reportingcategories = new List<M834ReportingCategory>();
            m834subids = new List<M834SubId>();
        }
        public M834File m834file { get; set; }
        public List<M834AdditionalName> m834additionalnames { get; set; }
        public List<M834Detail> m834details { get; set; }
        public List<M834DisabilityInfo> m834disabilityinfos { get; set; }
        public List<M834EmploymentClass> m834employmentclasses { get; set; }
        public List<M834HCCOBInfo> m834hccobinfos { get; set; }
        public List<M834HCProviderInfo> m834hcproviderinfos { get; set; }
        public List<M834HealthCoverage> m834healthcoverages { get; set; }
        public List<M834Language> m834languages { get; set; }
        public List<M834MemberLevelDate> m834memberleveldates { get; set; }
        public List<M834PolicyAmount> m834policyamounts { get; set; }
        public List<M834ReportingCategory> m834reportingcategories { get; set; }
        public List<M834SubId> m834subids { get; set; }
    }
}
