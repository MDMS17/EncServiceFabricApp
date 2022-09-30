using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.Premium820
{
    public class Member820
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public string EntityIdQualifier { get; set; }
        public string EntityId { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberIdQualifier { get; set; }
        public string MemberId { get; set; }
        public string InsuranceRemittanceReferenceNumber { get; set; }
        public string DetailPremiumPaymentAmount { get; set; }
        public string BilledPremiumAmount { get; set; }
        public string CountyCode { get; set; }
        public string OrganizationalReferenceId { get; set; }
        public string OrganizationalDescription { get; set; }
        public string CapitationFromDate { get; set; }
        public string CapitationThroughDate { get; set; }
        public string AdjustmentAmount { get; set; }
        public string AdjustmentReasonCode { get; set; }
    }
}
