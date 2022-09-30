using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsCode
    {
        public string ClaimID { get; set; }
        public string CodeType { get; set; }
        public string CodeSequence { get; set; }
        public string CodeLetter { get; set; }
        public string CodeValue { get; set; }
        public decimal? CodeAmount { get; set; }
        public DateTime? CodeStartDate { get; set; }
        public DateTime? CodeEndDate { get; set; }
        public string ICDTypeCode { get; set; }
        public string DIAGPOI { get; set; }
    }
}
