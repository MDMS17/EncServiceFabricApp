using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class N2_274 : X12SegmentBase 
    {
        public string AdditionalName1 { get; set; }
        public string AdditionalName2 { get; set; }
        public N2_274()
        {
            SegmentCode = "N2";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("N2*" + AdditionalName1);
            if (!string.IsNullOrEmpty(AdditionalName2)) sb.Append("*" + AdditionalName2);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
