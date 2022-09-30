using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Performed
    {
        public DateTime performedDateTime { get; set; }
        public Period performedPeriod { get; set; }
        public string performedString { get; set; }
        public uint performedAge { get; set; }
        public Range performedRange { get; set; }
    }
}
