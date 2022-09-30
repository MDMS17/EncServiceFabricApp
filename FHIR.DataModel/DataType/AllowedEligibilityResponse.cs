using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class AllowedEligibilityResponse
    {
        public uint? allowedUnsignedInt { get; set; }
        public string allowedString { get; set; }
        public Money allowedMoney { get; set; }
    }
}
