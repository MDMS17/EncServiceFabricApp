using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Meditrac
{
    public class MeditracLine
    {
        public int ClaimId { get; set; }
        public int LineNumber { get; set; }
        public string ProcedureCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string ProcedureDescription { get; set; }
        public decimal? LineChargeAmount { get; set; }
        public string UnitOfMeasure { get; set; }
        public Single? Quantity { get; set; }
        public string PlaceOfService { get; set; }
        public string DiagPointer1 { get; set; }
        public string DiagPointer2 { get; set; }
        public string DiagPointer3 { get; set; }
        public string DiagPointer4 { get; set; }
        public string EmergencyInd { get; set; }
        public string EPSDTInd { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string NationalDrugCode { get; set; }
        public Single? DrugQuantity { get; set; }
        public string DrugUnit { get; set; }
        public decimal? LinePaidAmount { get; set; }
        public string PaidDate { get; set; }
        public decimal? LineDeductAmount { get; set; }
        public decimal? LineCoinsuranceAmount { get; set; }
        public decimal? LineCopayAmount { get; set; }
        public decimal? LineCOBPaidAmount { get; set; }
        public string RevenueCode { get; set; }
    }
}
