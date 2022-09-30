using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class NX1_274 : X12SegmentBase
    {
        public string EntityIdCode { get; set; }
        public NX1_274() 
        {
            SegmentCode = "NX1";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "NX1*" + EntityIdCode + "~";
        }
    }
}
