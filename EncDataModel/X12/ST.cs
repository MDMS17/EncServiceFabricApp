using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class ST : X12SegmentBase
    {
        public string TransactionControlNumber { get; set; }
        public string VersionNumber { get; set; }
        public ST()
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
            return "ST*837*" + TransactionControlNumber + "*" + VersionNumber + "~";
        }
    }
}
