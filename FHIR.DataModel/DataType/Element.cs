using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Element
    {
        public string id { get; set; }
        public List<Extension> extension { get; set; }
    }
}
