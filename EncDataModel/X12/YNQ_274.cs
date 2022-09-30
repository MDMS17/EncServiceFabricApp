using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class YNQ_274 : X12SegmentBase
    {
        public string ConditionIndicator { get; set; }
        public string ResponseCode { get; set; }
        public YNQ_274() 
        {
            SegmentCode = "YNQ";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "YNQ*" + ConditionIndicator + "*" + ResponseCode + "~";
        }
    }
}
