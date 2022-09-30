using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class LQ_274 : X12SegmentBase 
    {
        public string TaxonomyQualifier { get; set; }
        public string TaxonomyCode { get; set; }
        public LQ_274() 
        {
            SegmentCode = "LQ";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String() 
        {
            return "LQ*" + TaxonomyQualifier + "*" + TaxonomyCode + "~";
        }
    }
}
