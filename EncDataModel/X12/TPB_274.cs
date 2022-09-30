using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class TPB_274 : X12SegmentBase 
    {
        public string RoleCode { get; set; }
        public TPB_274() 
        {
            SegmentCode = "TPB";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "TPB*" + RoleCode + "~";
        }
    }
}
