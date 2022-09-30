using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Reference
    {
        public string reference { get; set; }
        public string type { get; set; }
        public Identifier identifier { get; set; }
        public string display { get; set; }
    }
}
