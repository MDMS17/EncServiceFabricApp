using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class PDI_274 : X12SegmentBase 
    {
        public string RestrictionGender { get; set; }
        public string RestrictionAgeMin { get; set; }
        public string RestrictionAgeMax { get; set; }
        public PDI_274() 
        {
            SegmentCode = "PDI";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PDI");
            int emptyCount = 0;
            if (!string.IsNullOrEmpty(RestrictionGender)) sb.Append("*" + RestrictionGender);
            else emptyCount++;
            string s = new string('*', emptyCount + 1);
            if (!string.IsNullOrEmpty(RestrictionAgeMax)) sb.Append(s + RestrictionAgeMax);
            else emptyCount++;
            s = new string('*', emptyCount + 1);
            if (!string.IsNullOrEmpty(RestrictionAgeMin)) sb.Append(s + RestrictionAgeMin);
            sb.Append("~");
            return sb.ToString();
        }
    }
}
