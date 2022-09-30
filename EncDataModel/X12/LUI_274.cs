using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class LUI_274 : X12SegmentBase 
    {
        public string LanguageQualifier { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageProficiency { get; set; }
        public LUI_274() 
        {
            SegmentCode = "LUI";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "LUI*" + LanguageQualifier + "*" + LanguageCode + "***" + LanguageProficiency + "~";
        }
    }
}
