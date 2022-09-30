using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class SE : X12SegmentBase
    {
        public string SegmentCount { get; set; }
        public string TransactionControlNumber { get; set; }
        public SE()
        {
            SegmentCode = "SE";
            LoopName = "Trailer";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "SE*" + SegmentCount + "*" + TransactionControlNumber + "~";
        }
    }
}
