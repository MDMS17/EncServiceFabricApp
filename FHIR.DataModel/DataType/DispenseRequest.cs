using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DispenseRequest:BackboneElement
    {
        public InitialFill initialFill { get; set; }
        public TimeSpan dispenseInterval { get; set; }
        public Period validityPeriod { get; set; }
        public uint? numberOfRepeatsAllowed { get; set; }
        public decimal? quantity { get; set; }
        public TimeSpan expectedSupplyDuration { get; set; }
        public Reference performer { get; set; }
    }
}
