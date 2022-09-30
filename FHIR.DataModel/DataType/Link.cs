using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Link
    {
        public Reference other { get; set; }
        public string type { get; set; }
    }
}
