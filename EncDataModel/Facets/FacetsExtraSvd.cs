using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsExtraSvd
    {
        public string ClaimID { get; set; }
        public Int16 ClaimLineSeq { get; set; }
        public string MemberID { get; set; }
        public string COBPayerType { get; set; }
        public string COBPayerID { get; set; }
        public string COBPayerName { get; set; }
        public decimal? COBDtlAllowedAmt { get; set; }
        public decimal? COBDtlCoInsAmt { get; set; }
        public decimal? COBDtlCoPayAmt { get; set; }
        public decimal? COBDtlDedAmt { get; set; }
        public decimal? COBDtlPaidAmt { get; set; }
        public decimal? COBDtlDisAlwAmt { get; set; }
        public bool? IsHealthPlan { get; set; }
    }
}
