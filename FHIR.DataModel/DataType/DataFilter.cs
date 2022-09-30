using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DataFilter:Element
    {
        public string path { get; set; }
        public string searchParam { get; set; }
        public ValueDataRequirement value { get; set; }
    }
}
