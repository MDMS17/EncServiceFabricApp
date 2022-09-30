using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class CTP : X12SegmentBase
    {
        public string DrugQuantity { get; set; }
        public string DrugQualifier { get; set; }
        public CTP()
        {
            SegmentCode = "CTP";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(DrugQuantity) && !string.IsNullOrEmpty(DrugQualifier);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CTP****" + DrugQuantity + "*" + DrugQualifier);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
