using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsExtraSbr
    {
        public string ClaimID { get; set; }
        public string MemberID { get; set; }
        public string COBPayerID { get; set; }
        public string PayerFullName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerMiddleInitial { get; set; }
        public string PayerAddress1 { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerAddressCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string PayerCounty { get; set; }
        public string PayerCountryCode { get; set; }
        public bool? IsHealthPlan { get; set; }
        public string ClaimFilingIndicatorCode { get; set; }
        public string COBPayerType { get; set; }
    }
}
