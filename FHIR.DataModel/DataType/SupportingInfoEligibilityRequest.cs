using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SupportingInfoEligibilityRequest : BackboneElement
    {
        public uint sequence { get; set; }
        public Reference information { get; set; }
        public bool? appliesToAll { get; set; }
    }
}
