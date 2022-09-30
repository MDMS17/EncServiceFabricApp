using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.X12
{
    public class PER274_Item
    {
        public string FunctionCode { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string PhoneEx { get; set; }
    }
    public class PER_274 : X12SegmentBase
    {
        public List<PER274_Item> Pers { get; set; }
        public PER_274()
        {
            SegmentCode = "PER";
            Pers = new List<PER274_Item>();
        }
        public override bool Valid()
        {
            foreach (PER274_Item per in Pers)
            {
                if (string.IsNullOrEmpty(per.Email) && string.IsNullOrEmpty(per.Fax) && string.IsNullOrEmpty(per.Phone)) return false;
            }
            return true;
        }
        public override string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PER274_Item per in Pers)
            {
                sb.Append("PER*"+ per.FunctionCode+"*");
                if (!string.IsNullOrEmpty(per.ContactName)) sb.Append(per.ContactName);
                if (!string.IsNullOrEmpty(per.Phone)) sb.Append("*TE*" + per.Phone);
                if (!string.IsNullOrEmpty(per.PhoneEx)) sb.Append("*EX*" + per.PhoneEx);
                if (!string.IsNullOrEmpty(per.Email)) sb.Append("*EM*" + per.Email);
                if (!string.IsNullOrEmpty(per.Fax)) sb.Append("*FX*" + per.Fax);
                sb.Append("~");
            }
            return sb.ToString();
        }
    }
}
