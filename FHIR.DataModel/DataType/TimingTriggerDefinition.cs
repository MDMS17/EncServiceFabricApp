using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TimingTriggerDefinition
    {
        public Timing timingTiming { get; set; }
        public Reference timingReference { get; set; }
        public DateTime? timingDate { get; set; }
        public DateTime? timingDateTime { get; set; }
    }
}
