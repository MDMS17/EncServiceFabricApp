using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncDataModel.CMS277CA
{
    public class _277CALine
    {
        [Key]
        public long LineId { get; set; }

        public int FileId { get; set; }
        public string ClaimId { get; set; }
        public string ProcedureQual { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string LineChargeAmount { get; set; }
        public string RevenueCode { get; set; }
        public string UnitCount { get; set; }
        public string LineItemControlNumber { get; set; }
        public string PrescriptionNumber { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
    }
}
