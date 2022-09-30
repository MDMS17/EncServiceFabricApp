using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class DEG_274:X12SegmentBase 
    {
        public string DegreeCode { get; set; }
        public string DegreeDescription { get; set; }
        public DEG_274() 
        {
            SegmentCode = "DEG";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "DEG*" + DegreeCode + "***" + DegreeDescription + "~";
        }
    }
}
