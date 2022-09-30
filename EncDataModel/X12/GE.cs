using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class GE : X12SegmentBase
    {
        public string NumberofTransactionSets { get; set; }
        public string GroupControlNumber { get; set; }
        public GE()
        {
            SegmentCode = "GE";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(NumberofTransactionSets) && !string.IsNullOrEmpty(GroupControlNumber);
        }
        public override string ToX12String()
        {
            return "GE*" + NumberofTransactionSets + "*" + GroupControlNumber + "~";
        }
    }
}
