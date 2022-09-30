using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TimingAction
    {
        public DateTime? timingDateTime { get; set; }
        public Age timingAge { get; set; }
        public Period timingPeriod { get; set; }
        public TimeSpan timingDuration { get; set; }
        public Range timingRange { get; set; }
        public Timing timingTiming { get; set; }
    }
}
