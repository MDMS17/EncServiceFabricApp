using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsProvider
    {
        public string ClaimID { get; set; }
        public string ProviderType { get; set; }
        public string ProviderID { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderTaxID { get; set; }
        public string ProviderAddress1 { get; set; }
        public string ProviderAddress2 { get; set; }
        public string ProviderAddressCity { get; set; }
        public string ProviderAddressState { get; set; }
        public string ProviderAddressZip { get; set; }
        public string ProviderMedID { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderSpecialtyCode { get; set; }
        public string ProviderStatus { get; set; }
        public string ProviderTaxonomy { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime TerminateDate { get; set; }
        public string ProviderTelephone { get; set; }
    }
}
