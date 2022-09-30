using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Collected
    {
        public DateTime? collectedDateTime { get; set; }
        public Period collectedPeriod { get; set; }
    }
}
