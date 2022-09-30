using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class OnsetCondition
    {
        public uint? onsetAge { get; set; }
        public Range onsetRange { get; set; }
        public Period onsetPeriod { get; set; }
        public string onsetString { get; set; }
    }
}
