using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Schedule
    {
        public Timing scheduledTiming { get; set; }
        public Period scheduledPeriod { get; set; }
        public string scheduledString { get; set; }
    }
}
