using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class CRC_274 : X12SegmentBase 
    {
        public string Category { get; set; }
        public string Condition { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Code4 { get; set; }
        public string Code5 { get; set; }
        public CRC_274() 
        {
            SegmentCode = "CRC";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CRC*" + Category + "*" + Condition + "*" + Code1);
            if (!string.IsNullOrEmpty(Code2)) sb.Append("*" + Code2);
            if (!string.IsNullOrEmpty(Code3)) sb.Append("*" + Code3);
            if (!string.IsNullOrEmpty(Code4)) sb.Append("*" + Code4);
            if (!string.IsNullOrEmpty(Code5)) sb.Append("*" + Code5);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
