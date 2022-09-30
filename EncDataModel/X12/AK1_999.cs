using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class AK1_999 : X12SegmentBase
    {
        public string FunctionalCode { get; set; }
        public string GroupControlNumber { get; set; }
        public string VersionId { get; set; }
        public AK1_999() 
        {
            SegmentCode = "AK1";
            LoopName = "Header";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "AK1*"+FunctionalCode+"*"+GroupControlNumber+"*"+VersionId+"~";
        }
    }
}
