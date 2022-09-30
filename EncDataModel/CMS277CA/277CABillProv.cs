using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncDataModel.CMS277CA
{
    public class _277CABillProv
    {
        [Key]
        public long HeaderId { get; set; }

        public int FileId { get; set; }
        public string BillProvName { get; set; }
        public string BillProvIdQual { get; set; }
        public string BillProvId { get; set; }
        public string ClaimId { get; set; }
        public string BillProvSecondIdQual1 { get; set; }
        public string BillProvSecondId1 { get; set; }
        public string BillProvSecondIdQual2 { get; set; }
        public string BillProvSecondId2 { get; set; }
        public string BillProvSecondIdQual3 { get; set; }
        public string BillProvSecondId3 { get; set; }
        public string BillProvAcceptedQuantity { get; set; }
        public string BillProvRejectedQuantity { get; set; }
        public string BillProvAcceptedAmount { get; set; }
        public string BillProvRejectedAmount { get; set; }
    }
}
