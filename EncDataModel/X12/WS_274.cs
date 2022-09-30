using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class WS_274 : X12SegmentBase 
    {
        public string ShiftCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public WS_274() 
        {
            SegmentCode = "WS";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("WS*"+ShiftCode);
            if (!string.IsNullOrEmpty(StartTime)) sb.Append("*" + StartTime);
            if (!string.IsNullOrEmpty(EndTime)) sb.Append("*" + EndTime);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
