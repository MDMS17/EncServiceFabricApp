using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ValueDataRequirement
    {
        public DateTime? valueDateTime { get; set; }
        public Period valuePeriod { get; set; }
        public TimeSpan valueDuration { get; set; }
    }
}
