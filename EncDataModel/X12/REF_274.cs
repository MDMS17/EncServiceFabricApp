using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class REF_274 : X12SegmentBase 
    {
        public string ReferenceQualifier { get; set; }
        public string ReferenceId { get; set; }
        public REF_274() 
        {
            SegmentCode = "REF";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "REF*" + ReferenceQualifier + "*" + ReferenceId + "~";
        }
    }
}
