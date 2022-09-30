using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Reported
    {
        public bool? reportedBoolean { get; set; }
        public Reference reportedReference { get; set; }
    }
}
