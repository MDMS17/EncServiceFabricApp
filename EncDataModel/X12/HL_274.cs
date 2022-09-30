using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class HL_274 : X12SegmentBase 
    {
        public string HLID { get; set; }
        public string ParentID { get; set; }
        public string LevelCode { get; set; }
        public string ChildCode { get; set; }
        public HL_274()
        {
            SegmentCode = "HL";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "HL*"+HLID+"*"+ParentID +"*"+LevelCode +"*"+ChildCode +"~";
        }
    }
}
