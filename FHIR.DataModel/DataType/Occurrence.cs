using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Occurrence
    {
        public DateTime? occurrenceDateTime { get; set; }
        public Period occurrencePeriod { get; set; }
        public Timing occurrenceTiming { get; set; }
    }
}
