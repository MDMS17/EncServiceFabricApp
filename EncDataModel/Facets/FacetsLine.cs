using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsLine
    {
        public string ClaimID { get; set; }
        public Int16 ClaimSeqNbr { get; set; }
        public string POS { get; set; }
        public string ProcCode { get; set; }
        public bool? IsCap { get; set; }
        public string Mod1 { get; set; }
        public string Mod2 { get; set; }
        public string Mod3 { get; set; }
        public string Mod4 { get; set; }
        public decimal? ChargeAmt { get; set; }
        public Int16 Units { get; set; }
        public Int16 UnitsAllowed { get; set; }
        public decimal DtlDedAmt { get; set; }
        public decimal DtlCopayAmt { get; set; }
        public decimal DtlCoInsAmt { get; set; }
        public DateTime DtlDosFrom { get; set; }
        public DateTime DtlDosTo { get; set; }
        public decimal DtlAllowedAmt { get; set; }
        public decimal DtlProvPayAmt { get; set; }
        public decimal DtlMbrPayAmt { get; set; }
        public decimal DtlTotalPaidAmt { get; set; }
        public decimal DtlPaidAmt { get; set; }
        public decimal COBDtlAllowedAmt { get; set; }
        public decimal COBDtlPaidAmt { get; set; }
        public decimal COBDtlCoInsAmt { get; set; }
        public decimal COBDtlCoPayAmt { get; set; }
        public decimal COBDtlDedAmt { get; set; }
        public string RevCode { get; set; }
        public string DtlDiagCode { get; set; }
        public string DiagPointer1 { get; set; }
        public string DiagPointer2 { get; set; }
        public string DiagPointer3 { get; set; }
        public string DiagPointer4 { get; set; }
        public string DrugCode { get; set; }
        public decimal DrugUnits { get; set; }
        public string DrugUOM { get; set; }
        public string DtlCapInd { get; set; }
        public DateTime PaidDate { get; set; }
        public string ExcludeCode1 { get; set; }
        public string FamilyPlanIndicator { get; set; }
    }
}
