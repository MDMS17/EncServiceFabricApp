using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DeviceName : BackboneElement
    {
        public string name { get; set; }
        public string type { get; set; }
    }
}
