using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.Premium820
{
    public class File820
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string TotalPremiumAmount { get; set; }
        public string PaymentMethodQualifier { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }
        public string CheckIssueDate { get; set; }
        public string TraceTypeCode { get; set; }
        public string TraceNumber { get; set; }
        public string PayeeIdQualifier { get; set; }
        public string PayeeId { get; set; }
        public string CoverageFirstDate { get; set; }
        public string CoverageLastDate { get; set; }
        public string PayeeLastName { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
        public string PayerName { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
    }
}
