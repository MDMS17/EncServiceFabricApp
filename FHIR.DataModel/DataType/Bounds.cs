using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Bounds
    {
        public TimeSpan boundsDuration { get; set; }
        public Range boundsRange { get; set; }
        public Period boundsPeriod { get; set; }
    }
}
