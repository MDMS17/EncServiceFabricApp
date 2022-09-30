using System;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.MCPDIP
{
    public class ProcessLog
    {
        [Key]
        public int LogId { get; set; }
        public string TradingPartnerCode { get; set; }
        public string RecordYear { get; set; }
        public string RecordMonth { get; set; }
        public int? GrievanceTotal { get; set; }
        public int? GrievanceSubmits { get; set; }
        public int? GrievanceErrors { get; set; }
        public int? AppealTotal { get; set; }
        public int? AppealSubmits { get; set; }
        public int? AppealErrors { get; set; }
        public int? COCTotal { get; set; }
        public int? COCSubmits { get; set; }
        public int? COCErrors { get; set; }
        public int? OONTotal { get; set; }
        public int? OONSubmits { get; set; }
        public int? OONErrors { get; set; }
        public int? PCPATotal { get; set; }
        public int? PCPASubmits { get; set; }
        public int? PCPAErrors { get; set; }
        public string RunStatus { get; set; }
        public DateTime RunTime { get; set; }
        public string RunBy { get; set; }
    }

    public class SubmissionLog
    {
        [Key]
        public int SubmissionId { get; set; }
        public string RecordYear { get; set; }
        public string RecordMonth { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string SubmitterName { get; set; }
        public string SubmissionDate { get; set; }
        public string ValidationStatus { get; set; }
        public int? TotalGrievanceSubmitted { get; set; }
        public int? TotalGrievanceAccepted { get; set; }
        public int? TotalGrievanceRejected { get; set; }
        public int? TotalAppealSubmitted { get; set; }
        public int? TotalAppealAccepted { get; set; }
        public int? TotalAppealRejected { get; set; }
        public int? TotalCOCSubmitted { get; set; }
        public int? TotalCOCAccepted { get; set; }
        public int? TotalCOCRejected { get; set; }
        public int? TotalOONSubmitted { get; set; }
        public int? TotalOONAccepted { get; set; }
        public int? TotalOONRejected { get; set; }
        public int? TotalPCPASubmitted { get; set; }
        public int? TotalPCPAAccepted { get; set; }
        public int? TotalPCPARejected { get; set; }
        public int? ResponseHeaderId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    public class OperationLog
    {
        [Key]
        public long OperationLogId { get; set; }
        public string UserId { get; set; }
        public string ModuleName { get; set; }
        public string Message { get; set; }
        public DateTime OperationTime { get; set; }
    }
    public class IpaFileLog
    {
        [Key]
        public int Id { get; set; }
        public bool? Kaiser_PCPA_Received { get; set; }
        public bool? Kaiser_PCPA_Error { get; set; }
        public bool? Kaiser_PCPA_Correction { get; set; }
        public bool? Kaiser_MCPD_Received { get; set; }
        public bool? Kaiser_MCPD_Error { get; set; }
        public bool? Kaiser_MCPD_Correction { get; set; }
        public bool? A_MCPD_Received { get; set; }
        public bool? A_MCPD_Error { get; set; }
        public bool? A_MCPD_Correction { get; set; }
        public bool? B_MCPD_Received { get; set; }
        public bool? B_MCPD_Error { get; set; }
        public bool? B_MCPD_Correction { get; set; }
        public bool? E_MCPD_Received { get; set; }
        public bool? E_MCPD_Error { get; set; }
        public bool? E_MCPD_Correction { get; set; }
        public bool? F_MCPD_Received { get; set; }
        public bool? F_MCPD_Error { get; set; }
        public bool? F_MCPD_Correction { get; set; }
        public bool? N_MCPD_Received { get; set; }
        public bool? N_MCPD_Error { get; set; }
        public bool? N_MCPD_Correction { get; set; }
        public bool? Y_MCPD_Received { get; set; }
        public bool? Y_MCPD_Error { get; set; }
        public bool? Y_MCPD_Correction { get; set; }
        public string ReportingPeriod { get; set; }
    }
    public class TaxonomyCode 
    { 
        public string MEDICARE_SPECIALTY_CODE { get; set; }
        public string MEDICARE_PROVIDER { get; set; }
        [Key]
        public string TAXONOMY_CODE { get; set; }
        public string TAXONOMY_DESCRIPTION { get; set; }
    }
}
