using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class DMG : X12SegmentBase
    {
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public DMG()
        {
            SegmentCode = "DMG";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(BirthDate) && !string.IsNullOrEmpty(Gender);
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DMG*D8*" + BirthDate + "*" + Gender);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
