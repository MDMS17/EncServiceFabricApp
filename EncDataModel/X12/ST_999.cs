using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class ST_999 : X12SegmentBase
    {
        public string TransactionControlNumber { get; set; }
        public string VersionNumber { get; set; }
        public ST_999()
        {
            SegmentCode = "ST";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "ST*999*" + TransactionControlNumber + "*" + VersionNumber + "~";
        }
    }
}