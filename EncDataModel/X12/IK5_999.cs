using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class IK5_999 : X12SegmentBase
    {
        public string TransactionAckCode { get; set; }
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public string ErrorCode3 { get; set; }
        public string ErrorCode4 { get; set; }
        public string ErrorCode5 { get; set; }
        public IK5_999() 
        {
            SegmentCode = "IK5";
            LoopName = "2110";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            string ret = "IK5*" + TransactionAckCode;
            if (!string.IsNullOrEmpty(ErrorCode1)) ret += "*" + ErrorCode1;
            if (!string.IsNullOrEmpty(ErrorCode2)) ret += "*" + ErrorCode2;
            if (!string.IsNullOrEmpty(ErrorCode3)) ret += "*" + ErrorCode3;
            if (!string.IsNullOrEmpty(ErrorCode4)) ret += "*" + ErrorCode4;
            if (!string.IsNullOrEmpty(ErrorCode5)) ret += "*" + ErrorCode5;
            ret += "~";
            return ret;

        }
    }
}
