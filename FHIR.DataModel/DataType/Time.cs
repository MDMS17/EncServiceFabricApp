using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Time
    {
        public DateTime? timeDateTime { get; set; }
        public Period timePeriod { get; set; }
    }
}
