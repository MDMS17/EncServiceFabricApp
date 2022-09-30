using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class ISA_999 : X12SegmentBase
    {
        public string InterchangeSenderID { get; set; }
        public string InterchangeReceiverID { get; set; }
        public string InterchangeDate { get; set; }
        public string InterchangeTime { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string ProductionFlag { get; set; }
        public ISA_999()
        {
            SegmentCode = "ISA";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ISA*00*          *00*          *ZZ*");
            sb.Append(InterchangeSenderID.PadRight(15, ' '));
            sb.Append("*ZZ*" + InterchangeReceiverID.PadRight(15, ' '));
            sb.Append("*" + InterchangeDate.Substring(2, 6) + "*" + InterchangeTime + "*^*00501*" + InterchangeControlNumber + "*0*" + ProductionFlag + "*:~");
            return sb.ToString();
        }
    }
}
