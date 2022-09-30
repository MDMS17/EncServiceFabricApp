using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.Remittance835
{
    public class Claim835
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string PatientControlNumber { get; set; }
        public string ClaimStatus { get; set; }
        public string TotalChargeAmount { get; set; }
        public string TotalPaidAmount { get; set; }
        public string PatientResponsibilityAmount { get; set; }
        public string ClaimFilingIndicator { get; set; }
        public string PayerClaimcCntrolNumber { get; set; }
        public string FacilityTypeCode { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string PatientStatusCode { get; set; }
        public string DrgCode { get; set; }
        public string DrgWeight { get; set; }
        public string DischargePercent { get; set; }
        public string ClaimAdjustmentGroupCode { get; set; }
        public string AdjustmentReasoncCode1 { get; set; }
        public string AdjustmentAmount1 { get; set; }
        public string AdjustmentQuantity1 { get; set; }
        public string AdjustmentReasonCode2 { get; set; }
        public string AdjustmentAmount2 { get; set; }
        public string AdjustmentQuantity2 { get; set; }
        public string AdjustmentReasonCode3 { get; set; }
        public string AdjustmentAmount3 { get; set; }
        public string AdjustmentQuantity3 { get; set; }
        public string AdjustmentReasonCode4 { get; set; }
        public string AdjustmentAmount4 { get; set; }
        public string AdjustmentQuantity4 { get; set; }
        public string AdjustmentReasonCode5 { get; set; }
        public string AdjustmentAmount5 { get; set; }
        public string AdjustmentQuantity5 { get; set; }
        public string AdjustmentReasonCode6 { get; set; }
        public string AdjustmentAmount6 { get; set; }
        public string AdjustmentQuantity6 { get; set; }
        public string PatientCode { get; set; }
        public string PatientEntityType { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string PatientPrefix { get; set; }
        public string PatientSuffix { get; set; }
        public string PatientIdQualifier { get; set; }
        public string PatientId { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderEntityType { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderMiddleInitial { get; set; }
        public string ProviderPrefix { get; set; }
        public string ProviderSuffix { get; set; }
        public string ProviderIdQualifier { get; set; }
        public string ProviderId { get; set; }
        public string ExpirationDate { get; set; }
        public string ClaimReceivedDate { get; set; }
        public string SupplementalAmountQualifier { get; set; }
        public string SupplementalAmount { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public string LineChargeAmount { get; set; }
        public string LinePaidAmount { get; set; }
        public string RevenueCode { get; set; }
        public string PaidUnitCount { get; set; }
        public string DiffServiceTypeCode { get; set; }
        public string DiffProcedureCode { get; set; }
        public string DiffModifier1 { get; set; }
        public string DiffModifier2 { get; set; }
        public string DiffModifier3 { get; set; }
        public string DiffModifier4 { get; set; }
        public string DiffProcedureDescription { get; set; }
        public string ChargeUnitCount { get; set; }
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }
        public string ServiceDate { get; set; }
        public string LineAdjustmentGroupCode { get; set; }
        public string LineAdjustmentReasonCode1 { get; set; }
        public string LineAdjustmentAmount1 { get; set; }
        public string LineAdjustmentQuantity1 { get; set; }
        public string LineAdjustmentReasonCode2 { get; set; }
        public string LineAdjustmentAmount2 { get; set; }
        public string LineAdjustmentQuantity2 { get; set; }
        public string LineAdjustmentReasonCode3 { get; set; }
        public string LineAdjustmentAmount3 { get; set; }
        public string LineAdjustmentQuantity3 { get; set; }
        public string LineAdjustmentReasonCode4 { get; set; }
        public string LineAdjustmentAmount4 { get; set; }
        public string LineAdjustmentQuantity4 { get; set; }
        public string LineAdjustmentReasonCode5 { get; set; }
        public string LineAdjustmentAmount5 { get; set; }
        public string LineAdjustmentQuantity5 { get; set; }
        public string LineAdjustmentReasonCode6 { get; set; }
        public string LineAdjustmentAmount6 { get; set; }
        public string LineAdjustmentQuantity6 { get; set; }
        public string ProviderReferenceNumber { get; set; }
        public string LineItemControlNumber { get; set; }
        public string RenderingProviderNpi { get; set; }
        public string RenderingProviderTaxId { get; set; }
        public string LineAllowedAmount { get; set; }
        public string RemarkCode { get; set; }
    }
}
