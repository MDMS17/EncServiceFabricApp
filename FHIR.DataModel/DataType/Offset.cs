using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Offset
    {
        public TimeSpan offsetDuration { get; set; }
        public Range offsetRange { get; set; }
    }
}
