using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.Provider274
{
    public class P274File
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string TransactionSenderIdentifier { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string DirectoryExtractDate { get; set; }
        public string ContactFunctionCode { get; set; }
        public string ContactName { get; set; }
        public string ContactQualifier1 { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactQualifier2 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactQualifier3 { get; set; }
        public string ContactNumber3 { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ReportYear { get; set; }
        public string ReportMonth { get; set; }
    }

    public class P274Information
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string EntityIdCode { get; set; }
        public string EntityTypeQualifier { get; set; }
        public string EntityLastName { get; set; }
        public string EntityFirstName { get; set; }
        public string EntityIdQualifier { get; set; }
        public string EntityId { get; set; }
        public string LoopName { get; set; }
    }

    public class P274Detail
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string ProviderQualifier { get; set; }
        public string ProviderTypeQualifier { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderMiddleName { get; set; }
        public string ProviderSuffix { get; set; }
        public string ProviderIdQualifier { get; set; }
        public string ProviderId { get; set; }
        public string ProviderAdditionalName1 { get; set; }
        public string ProviderAdditionalName2 { get; set; }
        public string ProviderGender { get; set; }
        public string ProviderDegreeCode1 { get; set; }
        public string ProviderDegreeDescription1 { get; set; }
        public string ProviderDegreeCode2 { get; set; }
        public string ProviderDegreeDescription2 { get; set; }
        public string ProviderDegreeCode3 { get; set; }
        public string ProviderDegreeDescription3 { get; set; }
        public string ProviderDegreeCode4 { get; set; }
        public string ProviderDegreeDescription4 { get; set; }
        public string ProviderDegreeCode5 { get; set; }
        public string ProviderDegreeDescription5 { get; set; }
        public string ProviderDegreeCode6 { get; set; }
        public string ProviderDegreeDescription6 { get; set; }
        public string ProviderDegreeCode7 { get; set; }
        public string ProviderDegreeDescription7 { get; set; }
        public string ProviderDegreeCode8 { get; set; }
        public string ProviderDegreeDescription8 { get; set; }
        public string ProviderDegreeCode9 { get; set; }
        public string ProviderDegreeDescription9 { get; set; }
        public string ProviderLanguage1CodeQualifier { get; set; }
        public string ProviderLanguage1Code { get; set; }
        public string ProviderLanguage1ProficiencyInd { get; set; }
        public string ProviderLanguage2CodeQualifier { get; set; }
        public string ProviderLanguage2Code { get; set; }
        public string ProviderLanguage2ProficiencyInd { get; set; }
        public string ProviderLanguage3CodeQualifier { get; set; }
        public string ProviderLanguage3Code { get; set; }
        public string ProviderLanguage3ProficiencyInd { get; set; }
        public string ProviderLanguage4CodeQualifier { get; set; }
        public string ProviderLanguage4Code { get; set; }
        public string ProviderLanguage4ProficiencyInd { get; set; }
        public string ProviderLanguage5CodeQualifier { get; set; }
        public string ProviderLanguage5Code { get; set; }
        public string ProviderLanguage5ProficiencyInd { get; set; }
        public string ProviderLanguage6CodeQualifier { get; set; }
        public string ProviderLanguage6Code { get; set; }
        public string ProviderLanguage6ProficiencyInd { get; set; }
        public string ProviderLanguage7CodeQualifier { get; set; }
        public string ProviderLanguage7Code { get; set; }
        public string ProviderLanguage7ProficiencyInd { get; set; }
        public string ProviderLanguage8CodeQualifier { get; set; }
        public string ProviderLanguage8Code { get; set; }
        public string ProviderLanguage8ProficiencyInd { get; set; }
        public string ProviderLanguage9CodeQualifier { get; set; }
        public string ProviderLanguage9Code { get; set; }
        public string ProviderLanguage9ProficiencyInd { get; set; }
        public string ProviderContractEffectiveDate { get; set; }
        public string ProviderContractExpirationDate { get; set; }
        public string SiteContact1FunctionCode { get; set; }
        public string SiteContact1Qualifier1 { get; set; }
        public string SiteContact1Number1 { get; set; }
        public string SiteContact1Qualifier2 { get; set; }
        public string SiteContact1Number2 { get; set; }
        public string SiteContact1Qualifier3 { get; set; }
        public string SiteContact1Number3 { get; set; }
        public string SiteContact2FunctionCode { get; set; }
        public string SiteContact2Qualifier1 { get; set; }
        public string SiteContact2Number1 { get; set; }
        public string SiteContact2Qualifier2 { get; set; }
        public string SiteContact2Number2 { get; set; }
        public string SiteContact2Qualifier3 { get; set; }
        public string SiteContact2Number3 { get; set; }
        public string SitePatientAcceptanceCategory { get; set; }
        public string SitePatientAcceptanceCondition { get; set; }
        public string SitePatientAcceptanceCode { get; set; }
        public string SiteRestrictionGender { get; set; }
        public string SiteRestrictionMinAge { get; set; }
        public string SiteRestrictionMaxAge { get; set; }
        public string SiteLocationCode { get; set; }
        public string SiteLocationAddress { get; set; }
        public string SiteLocationAddress2 { get; set; }
        public string SiteLocationCity { get; set; }
        public string SiteLocationState { get; set; }
        public string SiteLocationZip { get; set; }
        public string SiteLocationCountry { get; set; }
        public string GroupId { get; set; }
        public string SiteId { get; set; }
    }

    public class P274AffiliatedEntity
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string EntityQualifier { get; set; }
        public string EntityTypeQualifier { get; set; }
        public string EntityLastName { get; set; }
        public string EntityIdQualifier { get; set; }
        public string EntityId { get; set; }
        public string EntityAdditionalName1 { get; set; }
        public string EntityAdditionalName2 { get; set; }
        public string HospitalStatusCode { get; set; }
        public string HospitalIdQualifier { get; set; }
        public string HospitalIdCode { get; set; }
    }

    public class P274SpecializationArea
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string LQQualifier { get; set; }
        public string LQCode { get; set; }
        public string NetworkRoleCode1 { get; set; }
        public string NetworkRoleCode2 { get; set; }
        public string CertificationConditionInd { get; set; }
        public string BoardCertificationInd { get; set; }
    }

    public class P274SiteCRC
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string Category { get; set; }
        public string Condition { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Code4 { get; set; }
        public string Code5 { get; set; }
    }

    public class P274GroupIdNumber
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string ReferenceQualifier { get; set; }
        public string ReferenceId { get; set; }
    }
    public class Provider274
    {
        public Provider274()
        {
            p274affiliatedentities = new List<P274AffiliatedEntity>();
            p274details = new List<P274Detail>();
            p274groupidnumbers = new List<P274GroupIdNumber>();
            p274informations = new List<P274Information>();
            p274sitecrcs = new List<P274SiteCRC>();
            p274specializationareas = new List<P274SpecializationArea>();
            p274siteworkschedules = new List<P274SiteWorkSchedule>();
        }
        public P274File p274file { get; set; }
        public List<P274AffiliatedEntity> p274affiliatedentities { get; set; }
        public List<P274Detail> p274details { get; set; }
        public List<P274GroupIdNumber> p274groupidnumbers { get; set; }
        public List<P274Information> p274informations { get; set; }
        public List<P274SiteCRC> p274sitecrcs { get; set; }
        public List<P274SpecializationArea> p274specializationareas { get; set; }
        public List<P274SiteWorkSchedule> p274siteworkschedules { get; set; }
    }
    public class P274SiteWorkSchedule
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string DetailId { get; set; }
        public string ShiftCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
