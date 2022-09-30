using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TimingClaim
    {
        public DateTime? timingDate { get; set; }
        public Period timingPeriod { get; set; }
    }
}
