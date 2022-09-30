using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class AK9_999 : X12SegmentBase
    {
        public string FunctionAckCode { get; set; }
        public string NumberofTransactions { get; set; }
        public string NumberOfReceivedTransactions { get; set; }
        public string NumberOfAccpetedTransactions { get; set; }
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public string ErrorCode3 { get; set; }
        public string ErrorCode4 { get; set; }
        public string ErrorCode5 { get; set; }
        public AK9_999() 
        {
            SegmentCode = "AK9";
            LoopName = "2110";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            string ret = "AK9*"+FunctionAckCode+"*"+NumberofTransactions+"*"+NumberOfReceivedTransactions+"*"+NumberOfAccpetedTransactions;
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
