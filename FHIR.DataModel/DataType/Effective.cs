using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Effective
    {
        public DateTime? effectiveDateTime { get; set; }
        public Period effectivePeriod { get; set; }
        public Timing effectiveTiming { get; set; }
        public DateTime? effectiveInstant { get; set; }
    }
}
