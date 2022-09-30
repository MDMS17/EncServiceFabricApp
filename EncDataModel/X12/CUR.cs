using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class CUR : X12SegmentBase
    {
        public string ProviderCurrencyCode { get; set; }
        public CUR()
        {
            SegmentCode = "CUR";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(ProviderCurrencyCode);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CUR*85*" + ProviderCurrencyCode);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
