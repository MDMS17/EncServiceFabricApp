using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Submission837
{
    public class CalculatedCounts
    {
        public int RecordCount { get; set; }
        public int ProcessedCount { get; set; }
        public int ExemptInProgressCount { get; set; }
        public int RejectedCount { get; set; }
        public int ExemptDuplicateCount { get; set; }
        public int ExemptMemberNotEligibleCount { get; set; }
        public int EligibileForIehpEditChecks { get; set; }
        public IList<ClaimValidationError> ValidationErrors { get; set; }
        public int InvalidCount { get; set; }
        public int ValidCount { get; set; }
        public DateTime Now { get; set; }
        public float Validity { get; set; }
        public string TransmissionmName { get; set; }
    }
    public class ClaimValidationError
    {
        public string Record { get; set; }
        public string ClaimId { get; set; }
        public int ErrorSequencePerEncounter { get; set; }
        public string ErrorId { get; set; }
        public string ErrorSeverityName { get; set; }
        public string ErrorDescription { get; set; }
        public string LoopNumber { get; set; }
        public string ElementName { get; set; }
    }
    public class DupLineI
    {
        public string MemberId { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeHour { get; set; }
        public string AttendingProvId { get; set; } //NPI, Medi-Cal Provider Id, State License Number
        public string RenderingProvId { get; set; } //NPI, Medi-Cal Provider Id, State License Number
        public string RevenueCode { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string NDC { get; set; }

    }
    public class DupLineP
    {
        public string MemberId { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string RenderingProvId { get; set; } //NPI, Medi-Cal ProviderId, State License Number
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string NDC { get; set; }
    }
    public class MemberDetail
    {
        public string ClaimId { get; set; }
        public string SubNumber { get; set; }
        public string PersNumber { get; set; }
        public string Gender { get; set; }
        public string LineOfBusiness { get; set; }
        public string HCP { get; set; }
        public string HIC { get; set; }
        public string MedsId { get; set; }
        public string FacilityCounty { get; set; }
        public string CIN { get; set; }
        public string MBI { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GroupNumber { get; set; }
    }
}
