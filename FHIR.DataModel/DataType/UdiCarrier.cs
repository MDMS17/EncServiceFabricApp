using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class UdiCarrier : BackboneElement
    {
        public string deviceIdentifier { get; set; }
        public string issuer { get; set; }
        public string jurisdiction { get; set; }
        public byte[] carrierAIDC { get; set; }
        public string carrierHRF { get; set; }
        public string entryType { get; set; }
    }
}
