using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class AK2_999 : X12SegmentBase
    {
        public string TransactionCode { get; set; }
        public string TransactionControlNumber { get; set; }
        public string VersionId { get; set; }
        public AK2_999() 
        {
            SegmentCode = "AK2";
            LoopName = "2000";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "AK2*"+TransactionCode+"*"+TransactionControlNumber+ (string.IsNullOrEmpty(VersionId)?"~": "*"+VersionId+"~");
        }
    }
}
