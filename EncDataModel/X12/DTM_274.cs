using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class DTM_274 : X12SegmentBase
    {
        public string TransactionDate { get; set; }
        public DTM_274() 
        {
            SegmentCode = "DTM";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "DTM*507*" + TransactionDate + "~";
        }
    }
}
