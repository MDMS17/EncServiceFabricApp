using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Extension : Element
    {
        public string url { get; set; }
        public string value { get; set; }
    }
}
