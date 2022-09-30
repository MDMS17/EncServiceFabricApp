using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.Remittance835
{
    public class File835
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string SenderIdQualifier { get; set; }
        public string SenderId { get; set; }
        public string ReceiverIdQualifier { get; set; }
        public string ReceiverId { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string ProductionFlag { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string MoneytaryAmount { get; set; }
        public string CreditFlag { get; set; }
        public string PaymentMethodCode { get; set; }
        public string PaymentFormatCode { get; set; }
        public string Sender_Dfi_Id_Qualifier { get; set; }
        public string Sender_Dfi_Id { get; set; }
        public string SenderAccountQualifier { get; set; }
        public string SenderAccountNumber { get; set; }
        public string OriginatingCompanyId { get; set; }
        public string OriginatingCompanySupplementalCode { get; set; }
        public string Receiver_Dfi_Id_Qualifier { get; set; }
        public string ReceiverBankId { get; set; }
        public string ReceiverAccountQualifier { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public string CheckIssueDate { get; set; }
        public string TraceTypeCode { get; set; }
        public string ReferenceId { get; set; }
        public string OriginatingComapnySupplementalCode { get; set; }
        public string ReceiverReferenceId { get; set; }
        public string PayerIdCode { get; set; }
        public string PayerName { get; set; }
        public string PayerIdQualifier { get; set; }
        public string PayerId { get; set; }
        public string PayerAddress { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string ContactFunctionCode { get; set; }
        public string ContactName { get; set; }
        public string ContactCommunicationQualifier { get; set; }
        public string ContactCommunicationNumber { get; set; }
        public string TechFunctionCode { get; set; }
        public string TechName { get; set; }
        public string TechCommunicationQualifier1 { get; set; }
        public string TechCommunicationNumber1 { get; set; }
        public string TechCommunicationQualifier2 { get; set; }
        public string TechCommunicationNumber2 { get; set; }
        public string WebFunctionCode { get; set; }
        public string WebName { get; set; }
        public string WebCommunicationQualifier { get; set; }
        public string WebCommunicationNumber { get; set; }
        public string PayeeIdCode { get; set; }
        public string PayeeName { get; set; }
        public string PayeeIdQualifier { get; set; }
        public string PayeeId { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeAddress2 { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
        public string PayeeAdditionalIdQualifier { get; set; }
        public string PayeeAdditionalId { get; set; }
        public string PayeeAdditionalIdQualifier2 { get; set; }
        public string PayeeAdditionalId2 { get; set; }
        public string ProviderAdjustmentId { get; set; }
        public string ProviderFiscalDate { get; set; }
        public string ProviderAdjustmentReasonCode { get; set; }
        public string ProviderAdjustmentId1 { get; set; }
        public string ProviderAdjustmentAmount1 { get; set; }
        public string ProviderAdjustmentId2 { get; set; }
        public string ProviderAdjustmentAmount2 { get; set; }
        public string ProviderAdjustmentId3 { get; set; }
        public string ProviderAdjustmentAmount3 { get; set; }
        public string ProviderAdjustmentId4 { get; set; }
        public string ProviderAdjustmentAmount4 { get; set; }
        public string ProviderAdjustmentId5 { get; set; }
        public string ProviderAdjustmentAmount5 { get; set; }
        public string ProviderAdjustmentId6 { get; set; }
        public string ProviderAdjustmentAmount6 { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
