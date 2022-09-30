using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncDataModel.CMS277CA
{
    public class _277CAStc
    {
        [Key]
        public long StcId { get; set; }

        public string StcType { get; set; }
        public int FileId { get; set; }
        public string ClaimId { get; set; }
        public string BillProvId { get; set; }
        public string PatientId { get; set; }
        public string ClaimStatusCategory1 { get; set; }
        public string ClaimStatusCode1 { get; set; }
        public string EntityIdentifier1 { get; set; }
        public string StatusInfoEffDate { get; set; }
        public string ActionCode { get; set; }
        public string ChargeAmount { get; set; }
        public string ClaimStatusCategory2 { get; set; }
        public string ClaimStatusCode2 { get; set; }
        public string EntityIdentifier2 { get; set; }
        public string ClaimStatusCategory3 { get; set; }
        public string ClaimStatusCode3 { get; set; }
        public string EntityIdentifier3 { get; set; }
    }
}
