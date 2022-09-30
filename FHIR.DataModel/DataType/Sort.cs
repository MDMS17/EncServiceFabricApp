using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Sort : Element
    {
        public string path { get; set; }
        public string direction { get; set; }
    }
}
